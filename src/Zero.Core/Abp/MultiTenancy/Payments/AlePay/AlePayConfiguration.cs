using Abp.Extensions;
using Microsoft.Extensions.Configuration;
using Zero.Configuration;

namespace Zero.MultiTenancy.Payments.AlePay
{
    public class AlePayPaymentGatewayConfiguration : IPaymentGatewayConfiguration
    {
        private readonly IConfigurationRoot _appConfiguration;

        public SubscriptionPaymentGatewayType GatewayType => SubscriptionPaymentGatewayType.AlePay;
        
        public bool IsActive => _appConfiguration["Payment:AlePay:IsActive"].To<bool>();

        public string BaseUrl => _appConfiguration["Payment:AlePay:BaseUrl"];

        public string TokenKey => _appConfiguration["Payment:AlePay:TokenKey"];

        public string ChecksumKey => _appConfiguration["Payment:AlePay:ChecksumKey"];
        
        public bool SupportsRecurringPayments => false;
        
        public AlePayPaymentGatewayConfiguration(IAppConfigurationAccessor configurationAccessor)
        {
            _appConfiguration = configurationAccessor.Configuration;
        }
    }
}