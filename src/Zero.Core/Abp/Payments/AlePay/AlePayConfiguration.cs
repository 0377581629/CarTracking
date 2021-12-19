using System;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Zero.Configuration;
using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Payments.AlePay
{
    public class AlePayPaymentGatewayConfiguration : IPaymentGatewayConfiguration
    {
        public SubscriptionPaymentGatewayType GatewayType => SubscriptionPaymentGatewayType.AlePay;

        public bool IsActive { get; }

        public bool IsActiveByConfig { get; }
        public string BaseUrl { get; }

        public string TokenKey { get; }

        public string ChecksumKey { get; }

        public bool SupportsRecurringPayments => false;

        public AlePayPaymentGatewayConfiguration(IMultiTenancyConfig multiTenancyConfig, IAppConfigurationAccessor configurationAccessor, ISettingManager settingManager, IAbpSession abpSession)
        {
            try
            {
                IsActiveByConfig = configurationAccessor.Configuration["Payment:AlePay:IsActive"].To<bool>();
                BaseUrl = configurationAccessor.Configuration["Payment:AlePay:BaseUrl"];
                TokenKey = configurationAccessor.Configuration["Payment:AlePay:TokenKey"];
                ChecksumKey = configurationAccessor.Configuration["Payment:AlePay:ChecksumKey"];

                #region Custom Config

                if (multiTenancyConfig.IsEnabled && abpSession.MultiTenancySide == MultiTenancySides.Tenant)
                {
                    var useCustomConfig = settingManager.GetSettingValueForTenant(AppSettings.PaymentManagement.UseCustomPaymentConfig, abpSession.GetTenantId(), true) == "true";
                    if (!useCustomConfig) return;
                    IsActive = settingManager.GetSettingValueForTenant(AppSettings.PaymentManagement.AlePayIsActive, abpSession.GetTenantId(), true) == "true";
                    if (!IsActive) return;
                    BaseUrl = settingManager.GetSettingValueForTenant(AppSettings.PaymentManagement.AlePayBaseUrl, abpSession.GetTenantId(), true);
                    TokenKey = settingManager.GetSettingValueForTenant(AppSettings.PaymentManagement.AlePayTokenKey, abpSession.GetTenantId(), true);
                    ChecksumKey = settingManager.GetSettingValueForTenant(AppSettings.PaymentManagement.AlePayChecksumKey, abpSession.GetTenantId(), true);
                }
                else
                {
                    var hostUseCustomConfig = settingManager.GetSettingValueForApplication(AppSettings.PaymentManagement.UseCustomPaymentConfig, true) == "true";
                    if (!hostUseCustomConfig) return;
                    IsActive = settingManager.GetSettingValueForApplication(AppSettings.PaymentManagement.AlePayIsActive, true) == "true";
                    if (!IsActive) return;
                    BaseUrl = settingManager.GetSettingValueForApplication(AppSettings.PaymentManagement.AlePayBaseUrl, true);
                    TokenKey = settingManager.GetSettingValueForApplication(AppSettings.PaymentManagement.AlePayTokenKey, true);
                    ChecksumKey = settingManager.GetSettingValueForApplication(AppSettings.PaymentManagement.AlePayChecksumKey, true);
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