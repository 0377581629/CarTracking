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
using Zero.Authorization;
using Zero.Authorization.Roles;
using Zero.Authorization.Users;
using Zero.Configuration;
using Zero.Identity;
using Zero.Web.Models.UserPayment;

namespace Zero.Web.Controllers
{
    public class UserPaymentController : ZeroControllerBase
    {
        #region Constructor
        private readonly IPaymentAppService _paymentAppService;
        private readonly ITenantRegistrationAppService _tenantRegistrationAppService;
        private readonly TenantManager _tenantManager;
        private readonly EditionManager _editionManager;
        private readonly IWebUrlService _webUrlService;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly UserClaimsPrincipalFactory<User, Role> _userClaimsPrincipalFactory;
        private readonly UserManager _userManager;
        private readonly SignInManager _signInManager;

        private readonly ISettingManager SettingManager;
        public UserPaymentController(
            IPaymentAppService paymentAppService,
            ITenantRegistrationAppService tenantRegistrationAppService,
            TenantManager tenantManager,
            EditionManager editionManager,
            IWebUrlService webUrlService,
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            UserClaimsPrincipalFactory<User, Role> userClaimsPrincipalFactory,
            UserManager userManager,
            SignInManager signInManager, 
            ISettingManager settingManager)
        {
            _paymentAppService = paymentAppService;
            _tenantRegistrationAppService = tenantRegistrationAppService;
            _tenantManager = tenantManager;
            _editionManager = editionManager;
            _webUrlService = webUrlService;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _userManager = userManager;
            _signInManager = signInManager;
            SettingManager = settingManager;
        }
        #endregion
        
        [AbpAuthorize]
        public async Task<IActionResult> ExtendSubscription()
        {
            var model = new ExtendUserSubscriptionViewModel
            {
                UserId = AbpSession.UserId.Value,
                UserEmail = (await _userManager.GetUserAsync(AbpSession.ToUserIdentifier())).EmailAddress,
                PaymentGateways = _paymentAppService.GetActiveGateways(new GetActiveGatewaysInput())
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
            // var paymentId = await _paymentAppService.CreatePayment(new CreatePaymentDto
            // {
            //     PaymentPeriodType = model.PaymentPeriodType,
            //     EditionId = model.EditionId,
            //     EditionPaymentType = model.EditionPaymentType,
            //     RecurringPaymentEnabled = model.RecurringPaymentEnabled.HasValue && model.RecurringPaymentEnabled.Value,
            //     SubscriptionPaymentGatewayType = model.Gateway,
            //     SuccessUrl = _webUrlService.GetSiteRootAddress().EnsureEndsWith('/') + "Payment/" + model.EditionPaymentType + "Succeed",
            //     ErrorUrl = _webUrlService.GetSiteRootAddress().EnsureEndsWith('/') + "Payment/PaymentFailed"
            // });

            return Json(new AjaxResponse
            {
                TargetUrl = Url.Action("Purchase", model.Gateway.ToString(), new
                {
                    // paymentId = paymentId,
                    // isUpgrade = model.EditionPaymentType == EditionPaymentType.Upgrade
                })
            });
        }

        [HttpPost]
        public async Task CancelPayment(CancelPaymentModel model)
        {
            await _paymentAppService.CancelPayment(new CancelPaymentDto
            {
                Gateway = model.Gateway,
                PaymentId = model.PaymentId
            });
        }
        
        public async Task<IActionResult> ExtendSucceed(long paymentId)
        {
            await _paymentAppService.ExtendSucceed(paymentId);

            return RedirectToAction("Index", "SubscriptionManagement", new { area = "App" });
        }

        public async Task<IActionResult> PaymentFailed(long paymentId)
        {
            await _paymentAppService.PaymentFailed(paymentId);

            if (AbpSession.UserId.HasValue)
            {
                return RedirectToAction("Index", "SubscriptionManagement", new { area = "App" });
            }

            return RedirectToAction("Index", "Home", new { area = "App" });
        }

        
        public IActionResult UserPaymentCompleted()
        {
            return View();
        }
    }
}