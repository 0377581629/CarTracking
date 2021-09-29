using Zero.Abp.Payments.AlePay;

namespace Zero.Web.Models.AlePay
{
    public class AlePayPurchaseViewModel
    {
        public long PaymentId { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
        
        public string Currency { get; set; }

        public AlePayPaymentGatewayConfiguration Configuration { get; set; }
        
        public string BuyerName { get; set; }
        
        public string BuyerEmail { get; set; }
        
        public string BuyerPhone { get; set; }
        
        public string BuyerAddress { get; set; }
        
        public string BuyerCity { get; set; }
        
        public string BuyerCountry { get; set; }
    }
}
