using alepay.Models;

namespace Zero.Abp.Payments.Dto
{
    public class AlePayCreatePaymentInput
    {
        public long PaymentId { get; set; }
        
        public RequestPaymentRequestModel RequestModel { get; set; }
    }
}
