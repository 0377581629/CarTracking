using System.Threading.Tasks;
using Abp.Dependency;
using Abp.UI;
using alepay;
using PayPalCheckoutSdk.Orders;
using Zero.MultiTenancy.Payments.AlePay;
using Zero.MultiTenancy.Payments.Paypal;

namespace Zero.Abp.MultiTenancy.Payments.AlePay
{
    public class AlePayGatewayManager : ZeroServiceBase, ITransientDependency
    {
        private readonly AlePayAPIClient _client;
        
        public AlePayGatewayManager(AlePayPaymentGatewayConfiguration configuration)
        {
            _client = new AlePayAPIClient(configuration.TokenKey,configuration.ChecksumKey);
        }

        public async Task<string> CreatePaymentRequest( input)
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