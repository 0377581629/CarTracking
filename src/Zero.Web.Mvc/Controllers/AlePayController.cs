using System;
using System.Threading;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using alepay;
using alepay.Models;
using Microsoft.AspNetCore.Mvc;
using Zero.Abp.Payments;
using Zero.Abp.Payments.AlePay;
using Zero.Abp.Payments.Dto;
using Zero.Authorization.Users;
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
        private readonly UserManager _userManager;
        private readonly IWebUrlService _webUrlService;
        public AlePayController(
            AlePayPaymentGatewayConfiguration alePayConfiguration,
            ISubscriptionPaymentRepository subscriptionPaymentRepository, 
            IAlePayPaymentAppService alePayPaymentAppService,
            IUserSubscriptionPaymentRepository userSubscriptionPaymentRepository,
            IWebUrlService webUrlService, 
            UserManager userManager)
        {
            _aleAlePayConfiguration = alePayConfiguration;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _alePayPaymentAppService = alePayPaymentAppService;
            _userSubscriptionPaymentRepository = userSubscriptionPaymentRepository;
            _webUrlService = webUrlService;
            _userManager = userManager;
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
                throw new ApplicationException("AlePay integration doesn't support recurring payments !");
            }

            var currentUser = await _userManager.GetUserAsync(AbpSession.ToUserIdentifier());
            
            var model = new AlePayPurchaseViewModel
            {
                PaymentId = payment.Id,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Description = payment.Description,
                Configuration = _aleAlePayConfiguration,
                
                BuyerName = currentUser.FullName,
                BuyerPhone = currentUser.PhoneNumber,
                BuyerEmail = currentUser.EmailAddress
            };

            return View(model);
        }
        
        [HttpPost]
        [UnitOfWork(IsDisabled = true)]
        public async Task<ActionResult> ConfirmUserPayment(AlePayPurchaseViewModel input)
        {
            var payment = await _userSubscriptionPaymentRepository.FirstOrDefaultAsync(input.PaymentId);
            if (payment == null)
                throw new UserFriendlyException(L("SubscriptionPaymentNotFound"));

            var cancelUrl = _webUrlService.GetSiteRootAddress().EnsureEndsWith('/') + "Payment/UserPaymentCancelled";
            
            var currentUser = await _userManager.GetUserAsync(AbpSession.ToUserIdentifier());
            
            var checkoutUrl = await _alePayPaymentAppService.CreateUserPayment(new AlePayCreatePaymentInput
            {
                PaymentId = input.PaymentId,
                RequestModel = new RequestPaymentRequestModel
                {
                    OrderCode = $"UserSubscription_{input.PaymentId}",
                    CustomMerchantId = currentUser.EmailAddress,
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
    }
}