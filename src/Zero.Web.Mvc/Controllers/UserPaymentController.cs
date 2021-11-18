using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Zero.Url;
using Zero.Web.Models.Payment;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Configuration;
using Zero.Abp.Payments;
using Zero.Abp.Payments.Dto;
using Zero.Authorization.Users;
using Zero.Configuration;
using Zero.Web.Models.UserPayment;

namespace Zero.Web.Controllers
{
    [AbpAuthorize]
    public class UserPaymentController : ZeroControllerBase
    {
        #region Constructor

        private readonly IUserPaymentAppService _userPaymentAppService;
        private readonly IWebUrlService _webUrlService;
        private readonly UserManager _userManager;
        private readonly ISettingManager _settingManager;

        public UserPaymentController(
            IUserPaymentAppService userPaymentAppService,
            IWebUrlService webUrlService,
            UserManager userManager,
            ISettingManager settingManager)
        {
            _userPaymentAppService = userPaymentAppService;
            _webUrlService = webUrlService;
            _userManager = userManager;
            _settingManager = settingManager;
        }

        #endregion

        public async Task<IActionResult> ExtendSubscription()
        {
            var model = new ExtendUserSubscriptionViewModel
            {
                UserId = AbpSession.UserId ?? 0,
                UserEmail = (await _userManager.GetUserAsync(AbpSession.ToUserIdentifier())).EmailAddress,
                PaymentGateways = await _userPaymentAppService.GetActiveGateways(new GetActiveGatewaysInput())
            };

            if (AbpSession.TenantId.HasValue)
            {
                model.MonthlyPrice =
                    await _settingManager.GetSettingValueForTenantAsync<double>(
                        AppSettings.UserManagement.SubscriptionMonthlyPrice, AbpSession.GetTenantId());
                model.YearlyPrice =
                    await _settingManager.GetSettingValueForTenantAsync<double>(
                        AppSettings.UserManagement.SubscriptionYearlyPrice, AbpSession.GetTenantId());
            }
            else
            {
                model.MonthlyPrice =
                    await _settingManager.GetSettingValueAsync<double>(AppSettings.UserManagement
                        .SubscriptionMonthlyPrice);
                model.YearlyPrice =
                    await _settingManager.GetSettingValueAsync<double>(AppSettings.UserManagement
                        .SubscriptionYearlyPrice);
            }

            return View("ExtendSubscription", model);
        }

        [HttpPost]
        public async Task<JsonResult> CreatePayment(CreateUserSubscriptionPaymentModel model)
        {
            var paymentId = await _userPaymentAppService.CreatePayment(new CreateUserPaymentDto
            {
                PaymentPeriodType = model.PaymentPeriodType,
                SubscriptionPaymentGatewayType = model.Gateway,
                SuccessUrl = _webUrlService.GetSiteRootAddress().EnsureEndsWith('/') + "UserPayment/ExtendSucceed",
                ErrorUrl = _webUrlService.GetSiteRootAddress().EnsureEndsWith('/') + "UserPayment/PaymentFailed"
            });

            return Json(new AjaxResponse
            {
                TargetUrl = Url.Action("UserPurchase", model.Gateway.ToString(), new
                {
                    paymentId
                })
            });
        }

        [HttpPost]
        public async Task CancelPayment(CancelPaymentModel model)
        {
            await _userPaymentAppService.CancelPayment(new CancelUserPaymentDto
            {
                Gateway = model.Gateway,
                PaymentId = model.PaymentId
            });
        }

        public async Task<IActionResult> ExtendSucceed(long paymentId)
        {
            await _userPaymentAppService.ExtendSucceed(paymentId);
            return RedirectToAction("Index", "UserSubscriptionManagement", new {area = "App"});
        }

        public async Task<IActionResult> PaymentFailed(long paymentId)
        {
            await _userPaymentAppService.PaymentFailed(paymentId);
            return RedirectToAction("Index", AbpSession.UserId.HasValue ? "UserSubscriptionManagement" : "Home",
                new {area = "App"});
        }
        
        public IActionResult UserPaymentCompleted()
        {
            return View();
        }
    }
}