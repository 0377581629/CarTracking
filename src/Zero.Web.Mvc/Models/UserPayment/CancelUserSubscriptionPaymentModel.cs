using Zero.MultiTenancy.Payments;

namespace Zero.Web.Models.UserPayment
{
    public class CancelUserSubscriptionPaymentModel
    {
        public string PaymentId { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }
    }
}