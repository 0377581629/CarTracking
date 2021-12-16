using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.UI;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;

namespace Zero.Abp.Payments.Paypal
{
    public class PayPalGatewayManager : ZeroServiceBase, ITransientDependency
    {
        private readonly PayPalHttpClient _client;
        
        public PayPalGatewayManager(PayPalPaymentGatewayConfiguration configuration)
        {
            var environment = GetEnvironment(configuration);
            _client = new PayPalHttpClient(environment);
        }

        private PayPalEnvironment GetEnvironment(PayPalPaymentGatewayConfiguration configuration)
        {
            return configuration.Environment switch
            {
                "sandbox" => new SandboxEnvironment(configuration.ClientId, configuration.ClientSecret),
                "live" => new LiveEnvironment(configuration.ClientId, configuration.ClientSecret),
                _ => new SandboxEnvironment(configuration.ClientId, configuration.ClientSecret)
            };
        }

        public async Task<string> CaptureOrderAsync(PayPalCaptureOrderRequestInput input)
        {
            var request = new OrdersCaptureRequest(input.OrderId);
            request.RequestBody(new OrderActionRequest());

            var response = await _client.Execute(request);
            var payment = response.Result<Order>();
            if (payment.Status != "COMPLETED")
            {
                throw new UserFriendlyException(L("PaymentFailed"));
            }

            return payment.Id;
        }
    }
}