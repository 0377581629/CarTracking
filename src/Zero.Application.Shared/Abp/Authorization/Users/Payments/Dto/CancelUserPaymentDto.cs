using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Authorization.Users.Payments.Dto
{
    public class CancelUserPaymentDto
    {
        public string PaymentId { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }
    }
}