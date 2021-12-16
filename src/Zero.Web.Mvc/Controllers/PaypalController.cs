using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zero.Abp.Payments;
using Zero.Abp.Payments.Paypal;
using Zero.Abp.Payments.PayPal;
using Zero.Customize;
using Zero.MultiTenancy.Payments;
using Zero.Web.Models.Paypal;

namespace Zero.Web.Controllers
{
    public class PayPalController : ZeroControllerBase
    {
        #region Constructor
        private readonly PayPalPaymentGatewayConfiguration _payPalConfiguration;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly IUserSubscriptionPaymentRepository _userSubscriptionPaymentRepository;
        private readonly IPayPalPaymentAppService _payPalPaymentAppService;
        private readonly IRepository<CurrencyRate> _currencyRateRepository;
        public PayPalController(
            PayPalPaymentGatewayConfiguration payPalConfiguration,
            ISubscriptionPaymentRepository subscriptionPaymentRepository, 
            IPayPalPaymentAppService payPalPaymentAppService,
            IUserSubscriptionPaymentRepository userSubscriptionPaymentRepository, 
            IRepository<CurrencyRate> currencyRateRepository)
        {
            _payPalConfiguration = payPalConfiguration;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _payPalPaymentAppService = payPalPaymentAppService;
            _userSubscriptionPaymentRepository = userSubscriptionPaymentRepository;
            _currencyRateRepository = currencyRateRepository;
        }
        #endregion
        
        #region Tenant Subscription
        public async Task<ActionResult> Purchase(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);
            if (payment.Status != SubscriptionPaymentStatus.NotPaid)
            {
                throw new ApplicationException("This payment is processed before");
            }

            if (payment.IsRecurring)
            {
                throw new ApplicationException("PayPal integration doesn't support recurring payments !");
            }
            var latestRate = await _currencyRateRepository.GetAll().OrderByDescending(o => o.Date).FirstOrDefaultAsync(o => o.SourceCurrency == "USD" && o.TargetCurrency == "VND");
            if (latestRate == null)
                throw new ApplicationException("Not found currency rate");
            var model = new PayPalPurchaseViewModel
            {
                PaymentId = payment.Id,
                Amount = payment.Amount,
                Currency = "USD",
                Description = payment.Description,
                Configuration = _payPalConfiguration
            };

            return View(model);
        }

        [HttpPost]
        [UnitOfWork(IsDisabled = true)]
        public async Task<ActionResult> ConfirmPayment(long paymentId, string paypalOrderId)
        {
            try
            {
                await _payPalPaymentAppService.ConfirmPayment(paymentId, paypalOrderId);
            
                var returnUrl = await GetSuccessUrlAsync(paymentId);
                return Redirect(returnUrl);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);

                var returnUrl = await GetErrorUrlAsync(paymentId);
                return Redirect(returnUrl);
            }
        }

        private async Task<string> GetSuccessUrlAsync(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);
            return payment.SuccessUrl + (payment.SuccessUrl.Contains("?") ? "&" : "?") + "paymentId=" + paymentId;
        }

        private async Task<string> GetErrorUrlAsync(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);
            return payment.ErrorUrl + (payment.ErrorUrl.Contains("?") ? "&" : "?") + "paymentId=" + paymentId;
        }
        
        [AbpAuthorize]
        public async Task<ActionResult> UserPurchase(long paymentId)
        {
            var payment = await _userSubscriptionPaymentRepository.GetAsync(paymentId);
            if (payment.Status != SubscriptionPaymentStatus.NotPaid)
            {
                throw new ApplicationException("This payment is processed before");
            }
            
            if (payment.IsRecurring)
            {
                throw new ApplicationException("PayPal integration doesn't support recurring payments !");
            }
            var latestRate = await _currencyRateRepository.GetAll().OrderByDescending(o => o.Date).FirstOrDefaultAsync(o => o.SourceCurrency == "USD" && o.TargetCurrency == "VND");
            if (latestRate == null)
                throw new ApplicationException("Not found currency rate");

            var model = new PayPalPurchaseViewModel
            {
                PaymentId = payment.Id,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Description = payment.Description,
                Configuration = _payPalConfiguration
            };

            return View(model);
        }
        #endregion
        
        #region User Subscription
        [HttpPost]
        [AbpAuthorize]
        [UnitOfWork(IsDisabled = true)]
        public async Task<ActionResult> UserConfirmPayment(long paymentId, string paypalOrderId)
        {
            try
            {
                await _payPalPaymentAppService.ConfirmUserPayment(paymentId, paypalOrderId);
                var returnUrl = await GetUserSuccessUrlAsync(paymentId);
                return Redirect(returnUrl);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
                var returnUrl = await GetUserErrorUrlAsync(paymentId);
                return Redirect(returnUrl);
            }
        }
        
        private async Task<string> GetUserSuccessUrlAsync(long paymentId)
        {
            var payment = await _userSubscriptionPaymentRepository.GetAsync(paymentId);
            return payment.SuccessUrl + (payment.SuccessUrl.Contains("?") ? "&" : "?") + "paymentId=" + paymentId;
        }

        private async Task<string> GetUserErrorUrlAsync(long paymentId)
        {
            var payment = await _userSubscriptionPaymentRepository.GetAsync(paymentId);
            return payment.ErrorUrl + (payment.ErrorUrl.Contains("?") ? "&" : "?") + "paymentId=" + paymentId;
        }
        #endregion
    }
}