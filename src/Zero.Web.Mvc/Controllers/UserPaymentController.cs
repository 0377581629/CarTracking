using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Zero.Editions;
using Zero.MultiTenancy;
using Zero.MultiTenancy.Dto;
using Zero.MultiTenancy.Payments;
using Zero.MultiTenancy.Payments.Dto;
using Zero.Url;
using Zero.Web.Models.Payment;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zero.Abp.Authorization.Payments;
using Zero.Abp.Authorization.Users.Payments;
using Zero.Abp.Authorization.Users.Payments.Dto;
using Zero.Authorization;
using Zero.Authorization.Roles;
using Zero.Authorization.Users;
using Zero.Configuration;
using Zero.Identity;
using Zero.Web.Models.UserPayment;

namespace Zero.Web.Controllers
{
    [AbpAuthorize]
    public class UserPaymentController : ZeroControllerBase
    {
        #region Constructor
        private readonly IUserPaymentAppService _userPaymentAppService;
        private readonly ITenantRegistrationAppService _tenantRegistrationAppService;
        private readonly TenantManager _tenantManager;
        private readonly EditionManager _editionManager;
        private readonly IWebUrlService _webUrlService;
        private readonly IUserSubscriptionPaymentRepository _userSubscriptionPaymentRepository;
        private readonly UserClaimsPrincipalFactory<User, Role> _userClaimsPrincipalFactory;
        private readonly UserManager _userManager;
        private readonly SignInManager _signInManager;

        private readonly ISettingManager SettingManager;
        public UserPaymentController(
            IUserPaymentAppService userPaymentAppService,
            ITenantRegistrationAppService tenantRegistrationAppService,
            TenantManager tenantManager,
            EditionManager editionManager,
            IWebUrlService webUrlService,
            IUserSubscriptionPaymentRepository userSubscriptionPaymentRepository,
            UserClaimsPrincipalFactory<User, Role> userClaimsPrincipalFactory,
            UserManager userManager,
            SignInManager signInManager, 
            ISettingManager settingManager)
        {
            _userPaymentAppService = userPaymentAppService;
            _tenantRegistrationAppService = tenantRegistrationAppService;
            _tenantManager = tenantManager;
            _editionManager = editionManager;
            _webUrlService = webUrlService;
            _userSubscriptionPaymentRepository = userSubscriptionPaymentRepository;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _userManager = userManager;
            _signInManager = signInManager;
            SettingManager = settingManager;
        }
        #endregion
        
        public async Task<IActionResult> ExtendSubscription()
        {
            var model = new ExtendUserSubscriptionViewModel
            {
                UserId = AbpSession.UserId.Value,
                UserEmail = (await _userManager.GetUserAsync(AbpSession.ToUserIdentifier())).EmailAddress,
                PaymentGateways = _userPaymentAppService.GetActiveGateways(new GetActiveGatewaysInput())
            };

            if (AbpSession.TenantId.HasValue)
            {
                model.MonthlyPrice = await SettingManager.GetSettingValueForTenantAsync<double>(AppSettings.UserManagement.SubscriptionMonthlyPrice, AbpSession.GetTenantId());
                model.YearlyPrice = await SettingManager.GetSettingValueForTenantAsync<double>(AppSettings.UserManagement.SubscriptionYearlyPrice, AbpSession.GetTenantId());
            }
            else
            {
                model.MonthlyPrice = await SettingManager.GetSettingValueAsync<double>(AppSettings.UserManagement.SubscriptionMonthlyPrice);
                model.YearlyPrice = await SettingManager.GetSettingValueAsync<double>(AppSettings.UserManagement.SubscriptionYearlyPrice);
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
            return RedirectToAction("Index", "SubscriptionManagement", new { area = "App" });
        }

        public async Task<IActionResult> PaymentFailed(long paymentId)
        {
            await _userPaymentAppService.PaymentFailed(paymentId);
            return RedirectToAction("Index", AbpSession.UserId.HasValue ? "UserSubscriptionManagement" : "Home", new { area = "App" });
        }

        
        public IActionResult UserPaymentCompleted()
        {
            return View();
        }
    }
}