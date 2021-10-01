using Zero.Editions;
using Zero.MultiTenancy.Payments;

namespace Zero.Web.Models.UserPayment
{
    public class CreateUserSubscriptionPaymentModel
    {
        public PaymentPeriodType? PaymentPeriodType { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }
    }
}
