using System;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Zero.Configuration;
using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Payments.Paypal
{
    public class PayPalPaymentGatewayConfiguration : IPaymentGatewayConfiguration
    {
        public SubscriptionPaymentGatewayType GatewayType => SubscriptionPaymentGatewayType.Paypal;

        public string Environment { get; }

        public string ClientId { get; }

        public string ClientSecret { get; }

        public string DemoUsername { get; }

        public string DemoPassword { get; }

        public bool IsActive { get; }

        public bool IsActiveByConfig { get; }

        public bool SupportsRecurringPayments => false;

        public PayPalPaymentGatewayConfiguration(IMultiTenancyConfig multiTenancyConfig, IAppConfigurationAccessor configurationAccessor, ISettingManager settingManager, IAbpSession abpSession)
        {
            try
            {
                IsActiveByConfig = configurationAccessor.Configuration["Payment:PayPal:IsActive"].To<bool>();
                Environment = configurationAccessor.Configuration["Payment:PayPal:Environment"];
                ClientId = configurationAccessor.Configuration["Payment:PayPal:ClientId"];
                ClientSecret = configurationAccessor.Configuration["Payment:PayPal:ClientSecret"];
                DemoUsername = configurationAccessor.Configuration["Payment:PayPal:DemoUsername"];
                DemoPassword = configurationAccessor.Configuration["Payment:PayPal:DemoPassword"];

                #region Custom Config

                if (multiTenancyConfig.IsEnabled && abpSession.MultiTenancySide == MultiTenancySides.Tenant)
                {
                    var useCustomConfig = settingManager.GetSettingValueForTenant(AppSettings.PaymentManagement.UseCustomPaymentConfig, abpSession.GetTenantId(), true) == "true";
                    if (!useCustomConfig) return;
                    IsActive = settingManager.GetSettingValueForTenant(AppSettings.PaymentManagement.PayPalIsActive, abpSession.GetTenantId(), true) == "true";
                    if (!IsActive) return;
                    Environment = settingManager.GetSettingValueForTenant(AppSettings.PaymentManagement.PayPalEnvironment, abpSession.GetTenantId(), true);
                    ClientId = settingManager.GetSettingValueForTenant(AppSettings.PaymentManagement.PayPalClientId, abpSession.GetTenantId(), true);
                    ClientSecret = settingManager.GetSettingValueForTenant(AppSettings.PaymentManagement.PayPalClientSecret, abpSession.GetTenantId(), true);
                    DemoUsername = settingManager.GetSettingValueForTenant(AppSettings.PaymentManagement.PayPalDemoUsername, abpSession.GetTenantId(), true);
                    DemoPassword = settingManager.GetSettingValueForTenant(AppSettings.PaymentManagement.PayPalDemoPassword, abpSession.GetTenantId(), true);
                }
                else
                {
                    var hostUseCustomConfig = settingManager.GetSettingValueForApplication(AppSettings.PaymentManagement.UseCustomPaymentConfig, true) == "true";
                    if (!hostUseCustomConfig) return;
                    IsActive = settingManager.GetSettingValueForApplication(AppSettings.PaymentManagement.PayPalIsActive, true) == "true";
                    if (!IsActive) return;
                    Environment = settingManager.GetSettingValueForApplication(AppSettings.PaymentManagement.PayPalEnvironment, true);
                    ClientId = settingManager.GetSettingValueForApplication(AppSettings.PaymentManagement.PayPalClientId, true);
                    ClientSecret = settingManager.GetSettingValueForApplication(AppSettings.PaymentManagement.PayPalClientSecret, true);
                    DemoUsername = settingManager.GetSettingValueForApplication(AppSettings.PaymentManagement.PayPalDemoUsername, true);
                    DemoPassword = settingManager.GetSettingValueForApplication(AppSettings.PaymentManagement.PayPalDemoPassword, true);
                }

                #endregion
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}