using System;
using System.Threading;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.UI;
using alepay;
using alepay.Models;
using Microsoft.AspNetCore.Mvc;
using Zero.Abp.Payments;
using Zero.Abp.Payments.AlePay;
using Zero.Abp.Payments.Dto;
using Zero.Customize;
using Zero.MultiTenancy.Payments;
using Zero.Url;
using Zero.Web.Models.AlePay;

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
        private readonly IWebUrlService _webUrlService;
        public AlePayController(
            AlePayPaymentGatewayConfiguration alePayConfiguration,
            ISubscriptionPaymentRepository subscriptionPaymentRepository, 
            IAlePayPaymentAppService alePayPaymentAppService,
            IUserSubscriptionPaymentRepository userSubscriptionPaymentRepository, 
            IRepository<CurrencyRate> currencyRateRepository, 
            IWebUrlService webUrlService)
        {
            _aleAlePayConfiguration = alePayConfiguration;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _alePayPaymentAppService = alePayPaymentAppService;
            _userSubscriptionPaymentRepository = userSubscriptionPaymentRepository;
            _currencyRateRepository = currencyRateRepository;
            _webUrlService = webUrlService;
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
        public async Task<ActionResult> ConfirmPayment(AlePayPurchaseViewModel input)
        {
            var payment = await _subscriptionPaymentRepository.FirstOrDefaultAsync(input.PaymentId);
            if (payment == null)
                throw new UserFriendlyException(L("SubscriptionPaymentNotFound"));

            var cancelUrl = _webUrlService.GetSiteRootAddress().EnsureEndsWith('/') + "Payment/PaymentCancelled";
            
            var checkoutUrl = await _alePayPaymentAppService.CreatePayment(new AlePayCreatePaymentInput
            {
                PaymentId = input.PaymentId,
                RequestModel = new RequestPaymentRequestModel
                {
                    OrderCode = $"TenantSubscription_{input.PaymentId}",
                    CustomMerchantId = "AnonymousCustomer",
                    Amount = Convert.ToDouble(payment.Amount),
                    Currency = "VND",
                    OrderDescription = payment.Description,
                    TotalItem = 1,
                    AllowDomestic = true,
                    
                    Language = Thread.CurrentThread.CurrentUICulture.Name=="vi"?"vi":"en",
                    
                    ReturnUrl = payment.SuccessUrl + (payment.SuccessUrl.Contains("?") ? "&" : "?") + "paymentId=" + payment.Id,
                    CancelUrl = cancelUrl + (cancelUrl.Contains("?") ? "&" : "?") + "paymentId=" + payment.Id,
                    
                    CheckoutType = AlePayDefs.CO_TYPE_INSTANT_PAYMENT_WITH_ATM_IB_QRCODE_INTL_CARDS,
                    
                    BuyerName = input.BuyerName,
                    BuyerEmail = input.BuyerEmail,
                    BuyerPhone = input.BuyerPhone,
                    BuyerAddress = input.BuyerAddress,
                    BuyerCity = input.BuyerCity,
                    BuyerCountry = input.BuyerCountry
                }
            });
            return Redirect(checkoutUrl);
        }
        
        // public async Task<ActionResult> UserPurchase(long paymentId)
        // {
        //     var payment = await _userSubscriptionPaymentRepository.GetAsync(paymentId);
        //     if (payment.Status != SubscriptionPaymentStatus.NotPaid)
        //     {
        //         throw new ApplicationException("This payment is processed before");
        //     }
        //     
        //     if (payment.IsRecurring)
        //     {
        //         throw new ApplicationException("PayPal integration doesn't support recurring payments !");
        //     }
        //     var latestRate = await _currencyRateRepository.GetAll().OrderByDescending(o => o.Date).FirstOrDefaultAsync(o => o.SourceCurrency == "USD" && o.TargetCurrency == "VND");
        //     if (latestRate == null)
        //         throw new ApplicationException("Not found currency rate");
        //
        //     var model = new PayPalPurchaseViewModel
        //     {
        //         PaymentId = payment.Id,
        //         Amount = payment.Amount,
        //         Currency = payment.Currency,
        //         Description = payment.Description,
        //         Configuration = _payPalConfiguration
        //     };
        //
        //     return View(model);
        // }
        //
        // [HttpPost]
        // [UnitOfWork(IsDisabled = true)]
        // public async Task<ActionResult> UserConfirmPayment(long paymentId, string paypalOrderId)
        // {
        //     try
        //     {
        //         await _payPalPaymentAppService.ConfirmUserPayment(paymentId, paypalOrderId);
        //     
        //         var returnUrl = await GetUserSuccessUrlAsync(paymentId);
        //         return Redirect(returnUrl);
        //     }
        //     catch (Exception exception)
        //     {
        //         Logger.Error(exception.Message, exception);
        //
        //         var returnUrl = await GetUserErrorUrlAsync(paymentId);
        //         return Redirect(returnUrl);
        //     }
        // }
        //
        // private async Task<string> GetUserSuccessUrlAsync(long paymentId)
        // {
        //     var payment = await _userSubscriptionPaymentRepository.GetAsync(paymentId);
        //     return payment.SuccessUrl + (payment.SuccessUrl.Contains("?") ? "&" : "?") + "paymentId=" + paymentId;
        // }
        //
        // private async Task<string> GetUserErrorUrlAsync(long paymentId)
        // {
        //     var payment = await _userSubscriptionPaymentRepository.GetAsync(paymentId);
        //     return payment.ErrorUrl + (payment.ErrorUrl.Contains("?") ? "&" : "?") + "paymentId=" + paymentId;
        // }
    }
}