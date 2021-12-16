using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Zero.Configuration;
using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Payments
{
    public class PaymentManager : ZeroServiceBase, IPaymentManager
    {
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ISettingManager _settingManager;
        private readonly IPaymentGatewayStore _paymentGatewayStore;
        private readonly IAbpSession _abpSession;

        public PaymentManager(IMultiTenancyConfig multiTenancyConfig, ISettingManager settingManager, IPaymentGatewayStore paymentGatewayStore, IAbpSession abpSession)
        {
            _multiTenancyConfig = multiTenancyConfig;
            _settingManager = settingManager;
            _paymentGatewayStore = paymentGatewayStore;
            _abpSession = abpSession;
        }


        public bool PaymentEnable()
        {
            var gatewaysByConfig = AllActivePaymentGatewayFromConfig();
            return gatewaysByConfig != null && gatewaysByConfig.Any();
        }

        public async Task<List<PaymentGatewayModel>> GetAllPaymentGatewayForSettings()
        {
            var gatewaysByConfig = AllActivePaymentGatewayFromConfig();
            if (gatewaysByConfig == null || !gatewaysByConfig.Any())
                return new List<PaymentGatewayModel>();
            if (!_multiTenancyConfig.IsEnabled || !_abpSession.TenantId.HasValue) return gatewaysByConfig;

            var allowTenantUseCustomConfig = await _settingManager.GetSettingValueForTenantAsync<bool>(AppSettings.PaymentManagement.AllowTenantUseCustomConfig, _abpSession.GetTenantId());

            return allowTenantUseCustomConfig ? gatewaysByConfig : new List<PaymentGatewayModel>();
        }

        public async Task<List<PaymentGatewayModel>> GetAllActivePaymentGateways()
        {
            if (!_multiTenancyConfig.IsEnabled) return await GetAllActivePaymentGatewaysInTenant(_abpSession.GetTenantId());
            if (_abpSession.MultiTenancySide != MultiTenancySides.Tenant) return await GetAllActivePaymentGatewaysInHost();
            if (await _settingManager.GetSettingValueForTenantAsync<bool>(AppSettings.PaymentManagement.AllowTenantUseCustomConfig, _abpSession.GetTenantId()))
                return await GetAllActivePaymentGatewaysInTenant(_abpSession.GetTenantId());
            return await GetAllActivePaymentGatewaysInHost();
        }

        private async Task<List<PaymentGatewayModel>> GetAllActivePaymentGatewaysInHost()
        {
            var gatewaysByConfig = AllActivePaymentGatewayFromConfig();
            if (gatewaysByConfig == null || !gatewaysByConfig.Any())
                return new List<PaymentGatewayModel>();

            var hostUseCustomConfig = await _settingManager.GetSettingValueForApplicationAsync<bool>(AppSettings.PaymentManagement.UseCustomPaymentConfig);
            var activePaymentGatewaysInHost = gatewaysByConfig;

            if (!hostUseCustomConfig) return activePaymentGatewaysInHost;

            var payPalIsActiveInHost = false;
            if (gatewaysByConfig.Any(o => o.GatewayType == SubscriptionPaymentGatewayType.Paypal))
                payPalIsActiveInHost = await _settingManager.GetSettingValueForApplicationAsync<bool>(AppSettings.PaymentManagement.PayPalIsActive);

            var alePayIsActiveInHost = false;
            if (gatewaysByConfig.Any(o => o.GatewayType == SubscriptionPaymentGatewayType.AlePay))
                alePayIsActiveInHost = await _settingManager.GetSettingValueForApplicationAsync<bool>(AppSettings.PaymentManagement.AlePayIsActive);

            activePaymentGatewaysInHost = gatewaysByConfig
                .WhereIf(!payPalIsActiveInHost, o => o.GatewayType != SubscriptionPaymentGatewayType.Paypal)
                .WhereIf(!alePayIsActiveInHost, o => o.GatewayType != SubscriptionPaymentGatewayType.AlePay)
                .ToList();

            return activePaymentGatewaysInHost;
        }

        private async Task<List<PaymentGatewayModel>> GetAllActivePaymentGatewaysInTenant(int tenantId)
        {
            var gatewaysByConfig = AllActivePaymentGatewayFromConfig();
            if (gatewaysByConfig == null || !gatewaysByConfig.Any())
                return new List<PaymentGatewayModel>();

            var useCustomConfig = await _settingManager.GetSettingValueForTenantAsync<bool>(AppSettings.PaymentManagement.UseCustomPaymentConfig, tenantId);
            if (!useCustomConfig) return gatewaysByConfig;
            var payPalIsActive = false;
            if (gatewaysByConfig.Any(o => o.GatewayType == SubscriptionPaymentGatewayType.Paypal))
                payPalIsActive = await _settingManager.GetSettingValueForTenantAsync<bool>(AppSettings.PaymentManagement.PayPalIsActive, tenantId);
            var alePayIsActive = false;
            if (gatewaysByConfig.Any(o => o.GatewayType == SubscriptionPaymentGatewayType.AlePay))
                alePayIsActive = await _settingManager.GetSettingValueForTenantAsync<bool>(AppSettings.PaymentManagement.AlePayIsActive, tenantId);
            return gatewaysByConfig
                .WhereIf(!payPalIsActive, o => o.GatewayType != SubscriptionPaymentGatewayType.Paypal)
                .WhereIf(!alePayIsActive, o => o.GatewayType != SubscriptionPaymentGatewayType.AlePay)
                .ToList();
        }

        private List<PaymentGatewayModel> AllActivePaymentGatewayFromConfig()
        {
            return _paymentGatewayStore.GetActiveGateways();
        }
    }
}