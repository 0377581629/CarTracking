using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Zero.Abp.Authorization.Users;
using Zero.Abp.Payments.AlePay;
using Zero.Abp.Payments.Dto;
using Zero.Authorization;
using Zero.Authorization.Users;
using Zero.Configuration;
using Zero.Customize.Interfaces;
using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Payments
{
    [AbpAuthorize]
    public class UserPaymentAppService : ZeroAppServiceBase, IUserPaymentAppService
    {
        private readonly IUserSubscriptionPaymentRepository _userSubscriptionPaymentRepository;

        private readonly IPaymentManager _paymentManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly UserManager _userManager;
        private readonly ISettingManager _settingManager;

        private readonly ICurrencyRateAppService _currencyRateAppService;
        private readonly IAlePayPaymentAppService _alePayPaymentAppService;

        public UserPaymentAppService(
            IUserSubscriptionPaymentRepository userSubscriptionPaymentRepository,
            IPaymentManager paymentManager,
            UserManager userManager,
            IRepository<User, long> userRepository,
            ISettingManager settingManager,
            ICurrencyRateAppService currencyRateAppService,
            IAlePayPaymentAppService alePayPaymentAppService)
        {
            _userSubscriptionPaymentRepository = userSubscriptionPaymentRepository;
            _paymentManager = paymentManager;
            _userManager = userManager;
            _userRepository = userRepository;
            _settingManager = settingManager;
            _currencyRateAppService = currencyRateAppService;
            _alePayPaymentAppService = alePayPaymentAppService;
        }

        public async Task<long> CreatePayment(CreateUserPaymentDto input)
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new ApplicationException(
                    "A user subscription payment only can be created for a user. UserId is not set in the IAbpSession!");
            }

            decimal amount = 0;
            double monthlyPrice = 0;
            double yearlyPrice = 0;
            string currency = ZeroConst.Currency;

            var user = await _userManager.GetUserAsync(AbpSession.ToUserIdentifier());

            if (AbpSession.TenantId.HasValue)
            {
                monthlyPrice =
                    await _settingManager.GetSettingValueForTenantAsync<double>(
                        AppSettings.UserManagement.SubscriptionMonthlyPrice, AbpSession.GetTenantId());
                yearlyPrice =
                    await _settingManager.GetSettingValueForTenantAsync<double>(
                        AppSettings.UserManagement.SubscriptionYearlyPrice, AbpSession.GetTenantId());
            }
            else
            {
                monthlyPrice =
                    await _settingManager.GetSettingValueAsync<double>(AppSettings.UserManagement
                        .SubscriptionMonthlyPrice);
                yearlyPrice =
                    await _settingManager.GetSettingValueAsync<double>(AppSettings.UserManagement
                        .SubscriptionYearlyPrice);
            }

            amount = input.PaymentPeriodType switch
            {
                PaymentPeriodType.Monthly => Convert.ToDecimal(monthlyPrice),
                PaymentPeriodType.Annual => Convert.ToDecimal(yearlyPrice),
                _ => amount
            };

            if (amount == 0)
                throw new UserFriendlyException(L("Invalid Amount To Create Payment"));

            // Convert to USD if gateway is Paypal or Stripe
            if (input.SubscriptionPaymentGatewayType == SubscriptionPaymentGatewayType.Paypal ||
                input.SubscriptionPaymentGatewayType == SubscriptionPaymentGatewayType.Stripe)
            {
                currency = "USD";
                var latestRate = await _currencyRateAppService.GetLatestRate();
                if (latestRate == null)
                    throw new UserFriendlyException(L("Not found currency rating data"));
                amount = amount / Convert.ToDecimal(latestRate.Value);
            }

            var payment = new UserSubscriptionPayment
            {
                UserId = AbpSession.GetUserId(),

                Description = GetPaymentDescription(input.PaymentPeriodType, user.EmailAddress, user.TenantId),
                PaymentPeriodType = input.PaymentPeriodType,

                Gateway = input.SubscriptionPaymentGatewayType,
                Amount = amount,
                Currency = currency,

                DayCount = input.PaymentPeriodType.HasValue ? (int) input.PaymentPeriodType.Value : 0,

                SuccessUrl = input.SuccessUrl,
                ErrorUrl = input.ErrorUrl
            };

            return await _userSubscriptionPaymentRepository.InsertAndGetIdAsync(payment);
        }

        public async Task CancelPayment(CancelUserPaymentDto input)
        {
            var payment = await _userSubscriptionPaymentRepository.GetByGatewayAndPaymentIdAsync(
                input.Gateway,
                input.PaymentId
            );

            payment.SetAsCancelled();
        }

        public async Task<PagedResultDto<UserSubscriptionPaymentListDto>> GetPaymentHistory(
            GetPaymentHistoryInput input)
        {
            var query = _userSubscriptionPaymentRepository.GetAll()
                .Include(sp => sp.User)
                .Where(sp => sp.UserId == AbpSession.GetUserId())
                .OrderBy(input.Sorting);

            var payments = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            var paymentsCount = query.Count();

            return new PagedResultDto<UserSubscriptionPaymentListDto>(paymentsCount,
                ObjectMapper.Map<List<UserSubscriptionPaymentListDto>>(payments));
        }

        public async Task<List<PaymentGatewayModel>> GetActiveGateways(GetActiveGatewaysInput input)
        {
            return (await _paymentManager.GetAllActivePaymentGateways())
                .WhereIf(input.RecurringPaymentsEnabled.HasValue,
                    gateway => input.RecurringPaymentsEnabled != null &&
                               gateway.SupportsRecurringPayments == input.RecurringPaymentsEnabled.Value)
                .ToList();
        }

        public async Task<UserSubscriptionPaymentDto> GetPaymentAsync(long paymentId)
        {
            return ObjectMapper.Map<UserSubscriptionPaymentDto>(
                await _userSubscriptionPaymentRepository.GetAsync(paymentId));
        }

        public async Task<UserSubscriptionPaymentDto> GetLastCompletedPayment()
        {
            var payment = await _userSubscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(
                userId: AbpSession.GetUserId(),
                gateway: null,
                isRecurring: null);

            return ObjectMapper.Map<UserSubscriptionPaymentDto>(payment);
        }

        public async Task ExtendSucceed(long paymentId)
        {
            var payment = await _userSubscriptionPaymentRepository.GetAsync(paymentId);

            if (payment.Gateway == SubscriptionPaymentGatewayType.AlePay)
            {
                var transactionInfo = await _alePayPaymentAppService.GetUserTransactionInfo(payment.Id);
                if (transactionInfo.Code == 0)
                {
                    payment.SetAsPaid();
                }
                else
                {
                    throw new UserFriendlyException("Your payment is not completed !");
                }
            }

            if (payment.Status != SubscriptionPaymentStatus.Paid)
            {
                throw new ApplicationException("Your payment is not completed !");
            }

            payment.SetAsCompleted();

            if (payment.PaymentPeriodType.HasValue)
            {
                var user = await _userRepository.GetAsync(payment.UserId);
                user.ExtendSubscriptionDate(payment.PaymentPeriodType.Value);
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task PaymentFailed(long paymentId)
        {
            var payment = await _userSubscriptionPaymentRepository.GetAsync(paymentId);
            payment.SetAsFailed();
        }

        public async Task PaymentCancelled(long paymentId)
        {
            var payment = await _userSubscriptionPaymentRepository.GetAsync(paymentId);
            payment.SetAsCancelled();
        }

        private string GetPaymentDescription(PaymentPeriodType? paymentPeriodType, string userEmail, int? tenantId)
        {
            var description = L("UserSubscription_Payment_Description", userEmail);

            if (tenantId.HasValue)
                description += " Tenant " + tenantId;

            return !paymentPeriodType.HasValue
                ? description
                : $"{description} {(int) paymentPeriodType} {L("Days").ToLower()}";
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement)]
        public async Task<bool> HasAnyPayment()
        {
            return await _userSubscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(
                userId: AbpSession.GetUserId(),
                gateway: null,
                isRecurring: null) != default;
        }
    }
}