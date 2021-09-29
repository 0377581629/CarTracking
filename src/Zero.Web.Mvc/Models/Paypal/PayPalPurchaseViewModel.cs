using Zero.Abp.Payments.Paypal;

namespace Zero.Web.Models.Paypal
{
    public class PayPalPurchaseViewModel
    {
        public long PaymentId { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
        
        public string Currency { get; set; }

        public PayPalPaymentGatewayConfiguration Configuration { get; set; }
    }
}
