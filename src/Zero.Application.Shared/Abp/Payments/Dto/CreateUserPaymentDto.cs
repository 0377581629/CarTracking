using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Payments.Dto
{
    public class CreateUserPaymentDto
    {
        public long UserId { get; set; }

        public PaymentPeriodType? PaymentPeriodType { get; set; }

        public SubscriptionPaymentGatewayType SubscriptionPaymentGatewayType { get; set; }

        public string SuccessUrl { get; set; }

        public string ErrorUrl { get; set; }
    }
}
