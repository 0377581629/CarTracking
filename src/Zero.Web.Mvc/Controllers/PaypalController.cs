using System;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Mvc;
using Zero.Abp.Authorization.Payments;
using Zero.MultiTenancy.Payments;
using Zero.MultiTenancy.Payments.Paypal;
using Zero.MultiTenancy.Payments.PayPal;
using Zero.Web.Models.Paypal;

namespace Zero.Web.Controllers
{
    public class PayPalController : ZeroControllerBase
    {
        private readonly PayPalPaymentGatewayConfiguration _payPalConfiguration;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly IUserSubscriptionPaymentRepository _userSubscriptionPaymentRepository;
        private readonly IPayPalPaymentAppService _payPalPaymentAppService;

        public PayPalController(
            PayPalPaymentGatewayConfiguration payPalConfiguration,
            ISubscriptionPaymentRepository subscriptionPaymentRepository, 
            IPayPalPaymentAppService payPalPaymentAppService, IUserSubscriptionPaymentRepository userSubscriptionPaymentRepository)
        {
            _payPalConfiguration = payPalConfiguration;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _payPalPaymentAppService = payPalPaymentAppService;
            _userSubscriptionPaymentRepository = userSubscriptionPaymentRepository;
            _payPalConfiguration = payPalConfiguration;
        }

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

            var model = new PayPalPurchaseViewModel
            {
                PaymentId = payment.Id,
                Amount = payment.Amount,
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

            var model = new PayPalPurchaseViewModel
            {
                PaymentId = payment.Id,
                Amount = payment.Amount,
                Description = payment.Description,
                Configuration = _payPalConfiguration
            };

            return View(model);
        }
        
        [HttpPost]
        [UnitOfWork(IsDisabled = true)]
        public async Task<ActionResult> UserConfirmPayment(long paymentId, string paypalOrderId)
        {
            try
            {
                await _payPalPaymentAppService.ConfirmPayment(paymentId, paypalOrderId);
            
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
    }
}