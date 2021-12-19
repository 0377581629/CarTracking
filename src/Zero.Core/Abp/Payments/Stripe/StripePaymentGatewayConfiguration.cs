using System;
using System.Collections.Generic;
using Abp.Extensions;
using Microsoft.Extensions.Configuration;
using Zero.Configuration;
using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Payments.Stripe
{
    public class StripePaymentGatewayConfiguration : IPaymentGatewayConfiguration
    {
        public SubscriptionPaymentGatewayType GatewayType => SubscriptionPaymentGatewayType.Stripe;

        public string BaseUrl { get; }

        public string PublishableKey { get; }

        public string SecretKey { get; }

        public string WebhookSecret { get; }

        public bool IsActive { get; }

        public bool IsActiveByConfig { get; }
        
        public bool SupportsRecurringPayments => true;

        public List<string> PaymentMethodTypes { get; }

        public StripePaymentGatewayConfiguration(IAppConfigurationAccessor configurationAccessor)
        {
            try
            {
                BaseUrl = configurationAccessor.Configuration["Payment:Stripe:BaseUrl"].EnsureEndsWith('/');
                PublishableKey = configurationAccessor.Configuration["Payment:Stripe:PublishableKey"];
                SecretKey = configurationAccessor.Configuration["Payment:Stripe:SecretKey"];
                WebhookSecret = configurationAccessor.Configuration["Payment:Stripe:WebhookSecret"];
                IsActive = configurationAccessor.Configuration["Payment:Stripe:IsActive"].To<bool>();
                IsActiveByConfig = configurationAccessor.Configuration["Payment:Stripe:IsActive"].To<bool>();
                PaymentMethodTypes = configurationAccessor.Configuration.GetSection("Payment:Stripe:PaymentMethodTypes").Get<List<string>>();
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}