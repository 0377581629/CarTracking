using Zero.MultiTenancy.Payments.Paypal;

namespace Zero.Web.Models.Paypal
{
    public class PayPalPurchaseViewModel
    {
        public long PaymentId { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
        
        public decimal AmountUsd { get; set; }

        public PayPalPaymentGatewayConfiguration Configuration { get; set; }
    }
}
