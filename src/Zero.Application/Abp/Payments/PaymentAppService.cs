using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Zero.Abp.Payments.AlePay;
using Zero.Abp.Payments.Dto;
using Zero.Authorization;
using Zero.Customize.Interfaces;
using Zero.Editions;
using Zero.Editions.Dto;
using Zero.MultiTenancy;
using Zero.MultiTenancy.Dto;
using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Payments
{
    public class PaymentAppService : ZeroAppServiceBase, IPaymentAppService
    {
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly EditionManager _editionManager;
        private readonly IPaymentManager _paymentManager;
        private readonly TenantManager _tenantManager;
        private readonly ICurrencyRateAppService _currencyRateAppService;
        private readonly IAlePayPaymentAppService _alePayPaymentAppService;
        public PaymentAppService(
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            EditionManager editionManager,
            IPaymentManager paymentManager,
            TenantManager tenantManager,
            ICurrencyRateAppService currencyRateAppService, 
            IAlePayPaymentAppService alePayPaymentAppService)
        {
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _editionManager = editionManager;
            _paymentManager = paymentManager;
            _tenantManager = tenantManager;
            _currencyRateAppService = currencyRateAppService;
            _alePayPaymentAppService = alePayPaymentAppService;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement)]
        public async Task<PaymentInfoDto> GetPaymentInfo(PaymentInfoInput input)
        {
            var tenant = await TenantManager.GetByIdAsync(AbpSession.GetTenantId());

            if (tenant.EditionId == null)
            {
                throw new UserFriendlyException(L("TenantEditionIsNotAssigned"));
            }

            var currentEdition = (SubscribableEdition)await _editionManager.GetByIdAsync(tenant.EditionId.Value);
            var targetEdition = input.UpgradeEditionId == null ? currentEdition : (SubscribableEdition)await _editionManager.GetByIdAsync(input.UpgradeEditionId.Value);

            decimal additionalPrice = 0;

            if (input.UpgradeEditionId.HasValue)
            {
                var lastPayment = await _subscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(
                    tenantId: AbpSession.GetTenantId(),
                    gateway: null,
                    isRecurring: null);

                using (UnitOfWorkManager.Current.SetTenantId(null))
                {
                    additionalPrice = await CalculateAmountForPaymentAsync(targetEdition, lastPayment.PaymentPeriodType, EditionPaymentType.Upgrade, tenant);
                }
            }

            var edition = ObjectMapper.Map<EditionSelectDto>(input.UpgradeEditionId == null ? currentEdition : targetEdition);

            return new PaymentInfoDto
            {
                Edition = edition,
                AdditionalPrice = additionalPrice
            };
        }

        public async Task<long> CreatePayment(CreatePaymentDto input)
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new ApplicationException("A payment only can be created for a tenant. TenantId is not set in the IAbpSession!");
            }

            decimal amount;
            string targetEditionName;
            string currency = ZeroConst.Currency;
            
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                var targetEdition = (SubscribableEdition)await _editionManager.GetByIdAsync(input.EditionId);
                targetEditionName = targetEdition.DisplayName;

                var tenant = await TenantManager.GetByIdAsync(AbpSession.GetTenantId());
                amount = await CalculateAmountForPaymentAsync(targetEdition, input.PaymentPeriodType, input.EditionPaymentType, tenant);
            }

            // Convert to USD if gateway is Paypal or Stripe
            if (input.SubscriptionPaymentGatewayType == SubscriptionPaymentGatewayType.Paypal || input.SubscriptionPaymentGatewayType == SubscriptionPaymentGatewayType.Stripe)
            {
                currency = "USD";
                var latestRate = await _currencyRateAppService.GetLatestRate();
                if (latestRate == null)
                    throw new UserFriendlyException(L("Not found currency rating data"));
                amount = amount / Convert.ToDecimal(latestRate.Value);
            }
            
            var payment = new SubscriptionPayment
            {
                Description = GetPaymentDescription(input.EditionPaymentType, input.PaymentPeriodType, targetEditionName, input.RecurringPaymentEnabled),
                PaymentPeriodType = input.PaymentPeriodType,
                EditionId = input.EditionId,
                TenantId = AbpSession.GetTenantId(),
                Gateway = input.SubscriptionPaymentGatewayType,
                Amount = amount,
                Currency = currency,
                DayCount = input.PaymentPeriodType.HasValue ? (int)input.PaymentPeriodType.Value : 0,
                IsRecurring = input.RecurringPaymentEnabled,
                SuccessUrl = input.SuccessUrl,
                ErrorUrl = input.ErrorUrl,
                EditionPaymentType = input.EditionPaymentType
            };

            return await _subscriptionPaymentRepository.InsertAndGetIdAsync(payment);
        }

        public async Task CancelPayment(CancelPaymentDto input)
        {
            var payment = await _subscriptionPaymentRepository.GetByGatewayAndPaymentIdAsync(
                    input.Gateway,
                    input.PaymentId
                );

            payment.SetAsCancelled();
        }

        public async Task<PagedResultDto<SubscriptionPaymentListDto>> GetPaymentHistory(GetPaymentHistoryInput input)
        {
            var query = _subscriptionPaymentRepository.GetAll()
                .Include(sp => sp.Edition)
                .Where(sp => sp.TenantId == AbpSession.GetTenantId())
                .OrderBy(input.Sorting);

            var payments = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            var paymentsCount = query.Count();

            return new PagedResultDto<SubscriptionPaymentListDto>(paymentsCount, ObjectMapper.Map<List<SubscriptionPaymentListDto>>(payments));
        }

        public async Task<List<PaymentGatewayModel>> GetActiveGateways(GetActiveGatewaysInput input)
        {
            return (await _paymentManager.GetAllActivePaymentGateways())
                .WhereIf(input.RecurringPaymentsEnabled.HasValue, gateway => gateway.SupportsRecurringPayments == input.RecurringPaymentsEnabled)
                .ToList();
        }

        public async Task<SubscriptionPaymentDto> GetPaymentAsync(long paymentId)
        {
            return ObjectMapper.Map<SubscriptionPaymentDto>(await _subscriptionPaymentRepository.GetAsync(paymentId));
        }

        public async Task<SubscriptionPaymentDto> GetLastCompletedPayment()
        {
            var payment = await _subscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(
                tenantId: AbpSession.GetTenantId(),
                gateway: null,
                isRecurring: null);

            return ObjectMapper.Map<SubscriptionPaymentDto>(payment);
        }

        public async Task BuyNowSucceed(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);

            if (payment.Gateway == SubscriptionPaymentGatewayType.AlePay)
            {
                var transactionInfo = await _alePayPaymentAppService.GetTransactionInfo(payment.Id);
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

            await _tenantManager.UpdateTenantAsync(
                payment.TenantId,
                true,
                false,
                payment.PaymentPeriodType,
                payment.EditionId,
                EditionPaymentType.BuyNow
            );
        }

        public async Task NewRegistrationSucceed(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);
            
            if (payment.Gateway == SubscriptionPaymentGatewayType.AlePay)
            {
                var transactionInfo = await _alePayPaymentAppService.GetTransactionInfo(payment.Id);
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

            await _tenantManager.UpdateTenantAsync(
                payment.TenantId,
                true,
                null,
                payment.PaymentPeriodType,
                payment.EditionId,
                EditionPaymentType.NewRegistration
            );
        }

        public async Task UpgradeSucceed(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);
            
            if (payment.Gateway == SubscriptionPaymentGatewayType.AlePay)
            {
                var transactionInfo = await _alePayPaymentAppService.GetTransactionInfo(payment.Id);
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

            await _tenantManager.UpdateTenantAsync(
                payment.TenantId,
                true,
                null,
                payment.PaymentPeriodType,
                payment.EditionId,
                EditionPaymentType.Upgrade
            );
        }

        public async Task ExtendSucceed(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);
            
            if (payment.Gateway == SubscriptionPaymentGatewayType.AlePay)
            {
                var transactionInfo = await _alePayPaymentAppService.GetTransactionInfo(payment.Id);
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

            await _tenantManager.UpdateTenantAsync(
                payment.TenantId,
                true,
                null,
                payment.PaymentPeriodType,
                payment.EditionId,
                EditionPaymentType.Extend
            );
        }

        public async Task PaymentFailed(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);
            payment.SetAsFailed();
        }

        public async Task PaymentCancelled(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);
            payment.SetAsCancelled();
        }
        
        private async Task<decimal> CalculateAmountForPaymentAsync(SubscribableEdition targetEdition, PaymentPeriodType? periodType, EditionPaymentType editionPaymentType, Tenant tenant)
        {
            if (editionPaymentType != EditionPaymentType.Upgrade)
            {
                return targetEdition.GetPaymentAmount(periodType);
            }

            if (tenant.EditionId == null)
            {
                throw new UserFriendlyException(L("CanNotUpgradeSubscriptionSinceTenantHasNoEditionAssigned"));
            }

            var remainingHoursCount = tenant.CalculateRemainingHoursCount();

            if (remainingHoursCount <= 0)
            {
                return targetEdition.GetPaymentAmount(periodType);
            }

            Debug.Assert(tenant.EditionId != null, "tenant.EditionId != null");

            var currentEdition = (SubscribableEdition)await _editionManager.GetByIdAsync(tenant.EditionId.Value);

            var lastPayment = await _subscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(tenant.Id, null, null);
            if (lastPayment?.PaymentPeriodType == null)
            {
                throw new ApplicationException("There is no completed payment record !");
            }

            return TenantManager.GetUpgradePrice(currentEdition, targetEdition, remainingHoursCount, lastPayment.PaymentPeriodType.Value);
        }

        private string GetPaymentDescription(EditionPaymentType editionPaymentType, PaymentPeriodType? paymentPeriodType, string targetEditionName, bool isRecurring)
        {
            var description = L(editionPaymentType + "_Edition_Description", targetEditionName);

            if (!paymentPeriodType.HasValue)
            {
                if (isRecurring && editionPaymentType == EditionPaymentType.Upgrade)
                {
                    description += " (" + L("CostOfProration") + ")";
                }

                return description;
            }

            if (editionPaymentType == EditionPaymentType.NewRegistration || editionPaymentType == EditionPaymentType.BuyNow)
            {
                description += " (" + L(paymentPeriodType.Value.ToString()) + ")";
            }

            if (isRecurring && editionPaymentType == EditionPaymentType.Upgrade)
            {
                description += " (" + L("CostOfProration") + ")";
            }

            return description;
        }

        public async Task SwitchBetweenFreeEditions(int upgradeEditionId)
        {
            var tenant = await _tenantManager.GetByIdAsync(AbpSession.GetTenantId());

            if (!tenant.EditionId.HasValue)
            {
                throw new ArgumentException("tenant.EditionId can not be null");
            }

            var currentEdition = _editionManager.GetById(tenant.EditionId.Value);
            if (!((SubscribableEdition)currentEdition).IsFree)
            {
                throw new ArgumentException("You can only switch between free editions. Current edition if not free");
            }

            var upgradeEdition = _editionManager.GetById(upgradeEditionId);
            if (!((SubscribableEdition)upgradeEdition).IsFree)
            {
                throw new ArgumentException("You can only switch between free editions. Target edition if not free");
            }

            await _tenantManager.UpdateTenantAsync(
                    tenant.Id,
                    true,
                    null,
                    null,
                    upgradeEditionId,
                    EditionPaymentType.Upgrade
                );
        }

        public async Task UpgradeSubscriptionCostsLessThenMinAmount(int editionId)
        {
            var paymentInfo = await GetPaymentInfo(new PaymentInfoInput { UpgradeEditionId = editionId });

            if (!paymentInfo.IsLessThanMinimumUpgradePaymentAmount())
            {
                throw new ApplicationException("Subscription payment requires more than minimum upgrade payment amount. Use payment gateway to charge payment amount.");
            }

            var lastPayment = await _subscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(
                tenantId: AbpSession.GetTenantId(),
                gateway: null,
                isRecurring: null);

            await _tenantManager.UpdateTenantAsync(
                AbpSession.GetTenantId(),
                true,
                null,
                lastPayment.GetPaymentPeriodType(),
                editionId,
                EditionPaymentType.Upgrade
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement)]
        public async Task<bool> HasAnyPayment()
        {
            return await _subscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(
                       tenantId: AbpSession.GetTenantId(),
                       gateway: null,
                       isRecurring: null) != default;
        }
    }
}
