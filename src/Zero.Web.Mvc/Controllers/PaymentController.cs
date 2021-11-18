using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Zero.Editions;
using Zero.MultiTenancy;
using Zero.MultiTenancy.Dto;
using Zero.MultiTenancy.Payments;
using Zero.Url;
using Zero.Web.Models.Payment;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Zero.Abp.Payments;
using Zero.Abp.Payments.Dto;
using Zero.Authorization;
using Zero.Authorization.Roles;
using Zero.Authorization.Users;
using Zero.Identity;

namespace Zero.Web.Controllers
{
    public class PaymentController : ZeroControllerBase
    {
        private readonly IPaymentAppService _paymentAppService;
        private readonly IUserPaymentAppService _userPaymentAppService;
        private readonly ITenantRegistrationAppService _tenantRegistrationAppService;
        private readonly TenantManager _tenantManager;
        private readonly EditionManager _editionManager;
        private readonly IWebUrlService _webUrlService;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly UserClaimsPrincipalFactory<User, Role> _userClaimsPrincipalFactory;
        private readonly UserManager _userManager;
        private readonly SignInManager _signInManager;

        public PaymentController(
            IPaymentAppService paymentAppService,
            ITenantRegistrationAppService tenantRegistrationAppService,
            TenantManager tenantManager,
            EditionManager editionManager,
            IWebUrlService webUrlService,
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            UserClaimsPrincipalFactory<User, Role> userClaimsPrincipalFactory,
            UserManager userManager,
            SignInManager signInManager, 
            IUserPaymentAppService userPaymentAppService)
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
            _userPaymentAppService = userPaymentAppService;
        }

        public async Task<IActionResult> Buy(int tenantId, int editionId, int? subscriptionStartType, int? editionPaymentType)
        {
            SetTenantIdCookie(tenantId);

            var edition = await _tenantRegistrationAppService.GetEdition(editionId);

            var model = new BuyEditionViewModel
            {
                Edition = edition,
                PaymentGateways = await _paymentAppService.GetActiveGateways(new GetActiveGatewaysInput())
            };

            if (editionPaymentType.HasValue)
            {
                model.EditionPaymentType = (EditionPaymentType)editionPaymentType.Value;
            }

            if (subscriptionStartType.HasValue)
            {
                model.SubscriptionStartType = (SubscriptionStartType)subscriptionStartType.Value;
            }

            return View("Buy", model);
        }

        public async Task<IActionResult> Upgrade(int upgradeEditionId)
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new ArgumentNullException();
            }

            SubscriptionPaymentType subscriptionPaymentType;

            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var tenant = await _tenantManager.GetByIdAsync(AbpSession.GetTenantId());
                subscriptionPaymentType = tenant.SubscriptionPaymentType;

                if (tenant.EditionId.HasValue)
                {
                    var currentEdition = await _editionManager.GetByIdAsync(tenant.EditionId.Value);
                    if (((SubscribableEdition)currentEdition).IsFree)
                    {
                        var upgradeEdition = await _editionManager.GetByIdAsync(upgradeEditionId);
                        if (((SubscribableEdition)upgradeEdition).IsFree)
                        {
                            await _paymentAppService.SwitchBetweenFreeEditions(upgradeEditionId);
                            return RedirectToAction("Index", "SubscriptionManagement", new { area = "App" });
                        }

                        return RedirectToAction("Buy", "Payment", new
                        {
                            tenantId = AbpSession.GetTenantId(),
                            editionId = upgradeEditionId,
                            editionPaymentType = (int)EditionPaymentType.BuyNow
                        });
                    }

                    if (!await _paymentAppService.HasAnyPayment())
                    {
                        return RedirectToAction("Buy", "Payment", new
                        {
                            tenantId = AbpSession.GetTenantId(),
                            editionId = upgradeEditionId,
                            editionPaymentType = (int)EditionPaymentType.BuyNow
                        });
                    }
                }
            }

            var paymentInfo = await _paymentAppService.GetPaymentInfo(new PaymentInfoInput { UpgradeEditionId = upgradeEditionId });

            if (paymentInfo.IsLessThanMinimumUpgradePaymentAmount())
            {
                await _paymentAppService.UpgradeSubscriptionCostsLessThenMinAmount(upgradeEditionId);
                return RedirectToAction("Index", "SubscriptionManagement", new { area = "App" });
            }
            var edition = await _tenantRegistrationAppService.GetEdition(upgradeEditionId);

            var lastPayment = await _subscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(
                tenantId: AbpSession.GetTenantId(),
                gateway: null,
                isRecurring: null);

            var model = new UpgradeEditionViewModel
            {
                Edition = edition,
                AdditionalPrice = paymentInfo.AdditionalPrice,
                SubscriptionPaymentType = subscriptionPaymentType,
                PaymentPeriodType = lastPayment.GetPaymentPeriodType()
            };

            if (subscriptionPaymentType.IsRecurring())
            {
                model.PaymentGateways = new List<PaymentGatewayModel>
                {
                    new()
                    {
                        GatewayType = lastPayment.Gateway,
                        SupportsRecurringPayments = true
                    }
                };
            }
            else
            {
                model.PaymentGateways = await _paymentAppService.GetActiveGateways(new GetActiveGatewaysInput());
            }

            return View("Upgrade", model);
        }

        public async Task<IActionResult> Extend(int upgradeEditionId, EditionPaymentType editionPaymentType)
        {
            var edition = await _tenantRegistrationAppService.GetEdition(upgradeEditionId);

            var model = new ExtendEditionViewModel
            {
                Edition = edition,
                PaymentGateways = await _paymentAppService.GetActiveGateways(new GetActiveGatewaysInput())
            };

            return View("Extend", model);
        }

        [HttpPost]
        public async Task<JsonResult> CreatePayment(CreatePaymentModel model)
        {
            var paymentId = await _paymentAppService.CreatePayment(new CreatePaymentDto
            {
                PaymentPeriodType = model.PaymentPeriodType,
                EditionId = model.EditionId,
                EditionPaymentType = model.EditionPaymentType,
                RecurringPaymentEnabled = model.RecurringPaymentEnabled.HasValue && model.RecurringPaymentEnabled.Value,
                SubscriptionPaymentGatewayType = model.Gateway,
                SuccessUrl = _webUrlService.GetSiteRootAddress().EnsureEndsWith('/') + "Payment/" + model.EditionPaymentType + "Succeed",
                ErrorUrl = _webUrlService.GetSiteRootAddress().EnsureEndsWith('/') + "Payment/PaymentFailed"
            });

            return Json(new AjaxResponse
            {
                TargetUrl = Url.Action("Purchase", model.Gateway.ToString(), new
                {
                    paymentId,
                    isUpgrade = model.EditionPaymentType == EditionPaymentType.Upgrade
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

        public async Task<IActionResult> BuyNowSucceed(long paymentId)
        {
            await _paymentAppService.BuyNowSucceed(paymentId);
            return RedirectToAction("Index", "SubscriptionManagement", new { area = "App" });
        }

        public async Task<IActionResult> NewRegistrationSucceed(long paymentId)
        {
            await _paymentAppService.NewRegistrationSucceed(paymentId);

            await LoginAdminAsync();

            return RedirectToAction("Index", "SubscriptionManagement", new { area = "App" });
        }

        public async Task<IActionResult> UpgradeSucceed(long paymentId)
        {
            await _paymentAppService.UpgradeSucceed(paymentId);

            return RedirectToAction("Index", "SubscriptionManagement", new { area = "App" });
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
        
        public async Task<IActionResult> PaymentCancelled(long paymentId)
        {
            await _paymentAppService.PaymentCancelled(paymentId);

            if (AbpSession.UserId.HasValue)
            {
                return RedirectToAction("Index", "SubscriptionManagement", new { area = "App" });
            }

            return RedirectToAction("Index", "Home", new { area = "App" });
        }

        public async Task<IActionResult> UserPaymentCancelled(long paymentId)
        {
            await _userPaymentAppService.PaymentCancelled(paymentId);

            if (AbpSession.UserId.HasValue)
            {
                return RedirectToAction("Index", "UserSubscriptionManagement", new { area = "App" });
            }

            return RedirectToAction("Index", "Home", new { area = "App" });
        }
        
        private async Task LoginAdminAsync()
        {
            var user = await _userManager.GetAdminAsync();
            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(principal.Identity as ClaimsIdentity, false);
        }

        public IActionResult PaymentCompleted()
        {
            return View();
        }
    }
}