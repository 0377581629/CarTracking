using Zero.MultiTenancy.Payments.AlePay;

namespace Zero.Web.Models.AlePay
{
    public class AlePayCreatePurchaseViewModel
    {
        public long PaymentId { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
        
        public string Currency { get; set; }

        public AlePayPaymentGatewayConfiguration Configuration { get; set; }
        
        
    }
}
