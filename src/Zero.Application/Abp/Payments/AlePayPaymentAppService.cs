using System.Threading.Tasks;
using Abp.UI;
using alepay;
using alepay.Models;
using Zero.Abp.Payments.AlePay;
using Zero.Abp.Payments.Dto;
using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Payments
{
    public class AlePayPaymentAppService : ZeroAppServiceBase, IAlePayPaymentAppService
    {
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly IUserSubscriptionPaymentRepository _userSubscriptionPaymentRepository;
        private readonly AlePayApiClient _alePayApiClient;
        public AlePayPaymentAppService(
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            IUserSubscriptionPaymentRepository userSubscriptionPaymentRepository, 
            AlePayPaymentGatewayConfiguration alePayConfiguration)
        {
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _userSubscriptionPaymentRepository = userSubscriptionPaymentRepository;
            _alePayApiClient = new AlePayApiClient( alePayConfiguration.BaseUrl, alePayConfiguration.TokenKey, alePayConfiguration.ChecksumKey);
        }

        public async Task<string> CreatePayment(AlePayCreatePaymentInput input)
        {
            ValidatePaymentRequest(input.RequestModel);
            var payment = await _subscriptionPaymentRepository.GetAsync(input.PaymentId);
            var paymentRequest = await _alePayApiClient.RequestPaymentAsync(input.RequestModel);
            payment.Gateway = SubscriptionPaymentGatewayType.AlePay;
            payment.ExternalPaymentId = paymentRequest.TransactionCode;
            if (paymentRequest.Code != 0) 
                throw new UserFriendlyException(L("PaymentGatewayCreateChargeFail"), paymentRequest.Message);
            return paymentRequest.CheckoutUrl;
        }

        public async Task<string> CreateUserPayment(AlePayCreatePaymentInput input)
        {
            ValidatePaymentRequest(input.RequestModel);
            var payment = await _userSubscriptionPaymentRepository.GetAsync(input.PaymentId);
            var paymentRequest = await _alePayApiClient.RequestPaymentAsync(input.RequestModel);
            payment.Gateway = SubscriptionPaymentGatewayType.AlePay;
            payment.ExternalPaymentId = paymentRequest.TransactionCode;
            if (paymentRequest.Code != 0) 
                throw new UserFriendlyException(L("PaymentGatewayCreateChargeFail"), paymentRequest.Message);
            return paymentRequest.CheckoutUrl;
        }


        public async Task<GetTransactionInfoResponseModel> GetTransactionInfo(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);
            if (payment.Gateway != SubscriptionPaymentGatewayType.AlePay)
                throw new UserFriendlyException(L("ThisPaymentNotProcessedByAlePay"));
            if (string.IsNullOrEmpty(payment.ExternalPaymentId))
                throw new UserFriendlyException(L("ThisPaymentNotProcessedByAlePayYet"));
            return await _alePayApiClient.GetTransactionInfoAsync(new GetTransactionInfoRequestModel
            {
                TransactionCode = payment.ExternalPaymentId
            });
        }
        
        public async Task<GetTransactionInfoResponseModel> GetUserTransactionInfo(long paymentId)
        {
            var payment = await _userSubscriptionPaymentRepository.GetAsync(paymentId);
            if (payment.Gateway != SubscriptionPaymentGatewayType.AlePay)
                throw new UserFriendlyException(L("ThisPaymentNotProcessedByAlePay"));
            if (string.IsNullOrEmpty(payment.ExternalPaymentId))
                throw new UserFriendlyException(L("ThisPaymentNotProcessedByAlePayYet"));
            return await _alePayApiClient.GetTransactionInfoAsync(new GetTransactionInfoRequestModel
            {
                TransactionCode = payment.ExternalPaymentId
            });
        }
        
        private void ValidatePaymentRequest(RequestPaymentRequestModel requestModel)
        {
            
        }
    }
}