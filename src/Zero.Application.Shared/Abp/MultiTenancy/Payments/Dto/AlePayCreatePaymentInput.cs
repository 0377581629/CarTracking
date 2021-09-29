using alepay.Models;

namespace Zero.MultiTenancy.Payments.Dto
{
    public class AlePayCreatePaymentInput
    {
        public long PaymentId { get; set; }
        
        public RequestPaymentRequestModel RequestModel { get; set; }
    }
}
