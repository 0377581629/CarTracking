using System.Threading.Tasks;
using alepay;
using alepay.Models;
using Zero.Abp.Authorization.Payments;
using Zero.Abp.MultiTenancy.Payments.AlePay;
using Zero.MultiTenancy.Payments;
using Zero.MultiTenancy.Payments.AlePay;
using Zero.MultiTenancy.Payments.Dto;
using Zero.MultiTenancy.Payments.Paypal;
using Zero.MultiTenancy.Payments.PayPal.Dto;

namespace Zero.Abp.MultiTenancy.Payments
{
    public class AlePayPaymentAppService : ZeroAppServiceBase, IAlePayPaymentAppService
    {
        private readonly AlePayPaymentGatewayConfiguration _alePayConfiguration;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly IUserSubscriptionPaymentRepository _userSubscriptionPaymentRepository;
        private readonly AlePayAPIClient _alePayApiClient;
        public AlePayPaymentAppService(
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            IUserSubscriptionPaymentRepository userSubscriptionPaymentRepository, 
            AlePayPaymentGatewayConfiguration alePayConfiguration)
        {
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _userSubscriptionPaymentRepository = userSubscriptionPaymentRepository;
            _alePayConfiguration = alePayConfiguration;
            _alePayApiClient = new AlePayAPIClient(_alePayConfiguration.TokenKey, _alePayConfiguration.ChecksumKey);
        }

        public async Task<string> CreatePayment(AlePayCreatePaymentInput input)
        {
            ValidatePaymentRequest(input.RequestModel);
            var payment = await _subscriptionPaymentRepository.GetAsync(input.PaymentId);
            var paymentRequest = await _alePayApiClient.RequestPaymentAsync(input.RequestModel);
            payment.Gateway = SubscriptionPaymentGatewayType.AlePay;
            payment.ExternalPaymentId = paymentRequest.TransactionCode;
            return paymentRequest.CheckoutUrl;
        }

        public async Task<string> CreateUserPayment(AlePayCreatePaymentInput input)
        {
            ValidatePaymentRequest(input.RequestModel);
            var payment = await _userSubscriptionPaymentRepository.GetAsync(input.PaymentId);
            var paymentRequest = await _alePayApiClient.RequestPaymentAsync(input.RequestModel);
            payment.Gateway = SubscriptionPaymentGatewayType.AlePay;
            payment.ExternalPaymentId = paymentRequest.TransactionCode;
            return paymentRequest.CheckoutUrl;
        }

        private void ValidatePaymentRequest(RequestPaymentRequestModel requestModel)
        {
            
        }
    }
}