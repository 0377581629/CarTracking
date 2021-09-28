using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zero.Abp.Authorization.Payments;
using Zero.Abp.MultiTenancy.Payments.AlePay;
using Zero.Customize;
using Zero.MultiTenancy.Payments;
using Zero.MultiTenancy.Payments.AlePay;
using Zero.MultiTenancy.Payments.Paypal;
using Zero.MultiTenancy.Payments.PayPal;
using Zero.Web.Models.AlePay;
using Zero.Web.Models.Paypal;

namespace Zero.Web.Controllers
{
    public class AlePayController : ZeroControllerBase
    {
        #region Constructor
        private readonly AlePayPaymentGatewayConfiguration _aleAlePayConfiguration;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly IUserSubscriptionPaymentRepository _userSubscriptionPaymentRepository;
        private readonly IAlePayPaymentAppService _alePayPaymentAppService;
        private readonly IRepository<CurrencyRate> _currencyRateRepository;
        public AlePayController(
            AlePayPaymentGatewayConfiguration alePayConfiguration,
            ISubscriptionPaymentRepository subscriptionPaymentRepository, 
            IAlePayPaymentAppService alePayPaymentAppService,
            IUserSubscriptionPaymentRepository userSubscriptionPaymentRepository, 
            IRepository<CurrencyRate> currencyRateRepository)
        {
            _aleAlePayConfiguration = alePayConfiguration;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _alePayPaymentAppService = alePayPaymentAppService;
            _userSubscriptionPaymentRepository = userSubscriptionPaymentRepository;
            _currencyRateRepository = currencyRateRepository;
        }
        #endregion
        
        public async Task<ActionResult> Purchase(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);
            if (payment.Status != SubscriptionPaymentStatus.NotPaid)
            {
                throw new ApplicationException("This payment is processed before");
            }
            
            if (payment.IsRecurring)
            {
                throw new ApplicationException("AlePay integration doesn't support recurring payments !");
            }
            
            var model = new AlePayPurchaseViewModel
            {
                PaymentId = payment.Id,
                Amount = payment.Amount,
                Currency = "VND",
                Description = payment.Description,
                Configuration = _aleAlePayConfiguration
            };

            return View(model);
        }

        [HttpPost]
        [UnitOfWork(IsDisabled = true)]
        public async Task<ActionResult> MakePayment(long paymentId)
        {
            try
            {
                await _alePayPaymentAppService.ConfirmPayment(paymentId, paypalOrderId);
                return Redirect(returnUrl);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
                return Redirect(returnUrl);
            }
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
            var latestRate = await _currencyRateRepository.GetAll().OrderByDescending(o => o.Date).FirstOrDefaultAsync(o => o.SourceCurrency == "USD" && o.TargetCurrency == "VND");
            if (latestRate == null)
                throw new ApplicationException("Not found currency rate");

            var model = new AlePayPurchaseViewModel
            {
                PaymentId = payment.Id,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Description = payment.Description,
                Configuration = _aleAlePayConfiguration
            };

            return View(model);
        }
        
        [HttpPost]
        [UnitOfWork(IsDisabled = true)]
        public async Task<ActionResult> UserConfirmPayment(long paymentId)
        {
            try
            {
                return Redirect(returnUrl);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
                return Redirect(returnUrl);
            }
        }
    }
}