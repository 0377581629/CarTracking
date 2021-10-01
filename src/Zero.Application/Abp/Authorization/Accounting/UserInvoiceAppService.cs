using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using Zero.Abp.Authorization.Accounting;
using Zero.Abp.Authorization.Accounting.Dto;
using Zero.Abp.Payments;
using Zero.Authorization.Users;
using Zero.Configuration;
using Zero.Editions;
using Zero.MultiTenancy.Accounting.Dto;
using Zero.MultiTenancy.Payments;

namespace Zero.MultiTenancy.Accounting
{
    public class UserInvoiceAppService : ZeroAppServiceBase, IUserInvoiceAppService
    {
        private readonly IUserSubscriptionPaymentRepository _subscriptionPaymentRepository;
        
        private readonly IInvoiceNumberGenerator _invoiceNumberGenerator;
        private readonly IRepository<UserInvoice> _invoiceRepository;
        private readonly UserManager _userManager;

        public UserInvoiceAppService(
            IInvoiceNumberGenerator invoiceNumberGenerator,
            IRepository<UserInvoice> invoiceRepository, 
            IUserSubscriptionPaymentRepository userSubscriptionPaymentRepository, 
            UserManager userManager)
        {
            _invoiceNumberGenerator = invoiceNumberGenerator;
            _invoiceRepository = invoiceRepository;
            _subscriptionPaymentRepository = userSubscriptionPaymentRepository;
            _userManager = userManager;
        }

        public async Task<UserInvoiceDto> GetInvoiceInfo(EntityDto<long> input)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(input.Id);

            if (string.IsNullOrEmpty(payment.InvoiceNo))
            {
                throw new Exception("There is no invoice for this payment !");
            }

            if (payment.UserId != AbpSession.GetUserId())
            {
                throw new UserFriendlyException(L("ThisInvoiceIsNotYours"));
            }

            var invoice = await _invoiceRepository.FirstOrDefaultAsync(b => b.InvoiceNo == payment.InvoiceNo);
            if (invoice == null)
            {
                throw new UserFriendlyException();
            }

            return new UserInvoiceDto
            {
                InvoiceNo = payment.InvoiceNo,
                InvoiceDate = invoice.InvoiceDate,
                Amount = payment.Amount
            };
        }

        [UnitOfWork(IsolationLevel.ReadUncommitted)]
        public async Task CreateInvoice(CreateUserInvoiceDto input)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(input.UserSubscriptionPaymentId);
            if (!string.IsNullOrEmpty(payment.InvoiceNo))
            {
                throw new Exception("Invoice is already generated for this payment.");
            }

            var invoiceNo = await _invoiceNumberGenerator.GetNewInvoiceNumber();

            var tenantLegalName = await SettingManager.GetSettingValueAsync(AppSettings.TenantManagement.BillingLegalName);
            var tenantAddress = await SettingManager.GetSettingValueAsync(AppSettings.TenantManagement.BillingAddress);
            var tenantTaxNo = await SettingManager.GetSettingValueAsync(AppSettings.TenantManagement.BillingTaxVatNo);

            if (string.IsNullOrEmpty(tenantLegalName) || string.IsNullOrEmpty(tenantAddress) || string.IsNullOrEmpty(tenantTaxNo))
            {
                throw new UserFriendlyException(L("InvoiceInfoIsMissingOrNotCompleted"));
            }

            await _invoiceRepository.InsertAsync(new UserInvoice
            {
                InvoiceNo = invoiceNo,
                InvoiceDate = Clock.Now
            });

            payment.InvoiceNo = invoiceNo;
        }
    }
}