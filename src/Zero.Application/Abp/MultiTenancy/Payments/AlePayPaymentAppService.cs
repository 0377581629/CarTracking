using System.Threading.Tasks;
using Zero.Abp.Authorization.Payments;
using Zero.Abp.MultiTenancy.Payments.AlePay;
using Zero.MultiTenancy.Payments;
using Zero.MultiTenancy.Payments.Paypal;
using Zero.MultiTenancy.Payments.PayPal.Dto;

namespace Zero.Abp.MultiTenancy.Payments
{
    public class AlePayPaymentAppService : ZeroAppServiceBase, IAlePayPaymentAppService
    {
        private readonly AlePayGatewayManager _alePayGatewayManager;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly IUserSubscriptionPaymentRepository _userSubscriptionPaymentRepository;

        public AlePayPaymentAppService(
            AlePayGatewayManager alePayGatewayManager,
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            IUserSubscriptionPaymentRepository userSubscriptionPaymentRepository)
        {
            _alePayGatewayManager = alePayGatewayManager;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _userSubscriptionPaymentRepository = userSubscriptionPaymentRepository;
        }

        public async Task<string> CreatePayment(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);

            await _alePayGatewayManager.CaptureOrderAsync(
                new PayPalCaptureOrderRequestInput(paypalOrderId)
            );

            payment.Gateway = SubscriptionPaymentGatewayType.Paypal;
            payment.ExternalPaymentId = paypalOrderId;
            payment.SetAsPaid();
        }

        public async Task<string> CreateUserPayment(long paymentId)
        {
            var payment = await _userSubscriptionPaymentRepository.GetAsync(paymentId);

            await _alePayGatewayManager.CaptureOrderAsync(
                new PayPalCaptureOrderRequestInput(paypalOrderId)
            );

            payment.Gateway = SubscriptionPaymentGatewayType.Paypal;
            payment.ExternalPaymentId = paypalOrderId;
            payment.SetAsPaid();
        }
    }
}