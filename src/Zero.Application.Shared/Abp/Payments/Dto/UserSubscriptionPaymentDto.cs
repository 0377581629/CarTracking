using Abp.Application.Services.Dto;
using Zero.Editions;
using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Payments.Dto
{
    public class UserSubscriptionPaymentDto : EntityDto<long>
    {
        public string Description { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }

        public decimal Amount { get; set; }

        public long UserId { get; set; }

        public int DayCount { get; set; }

        public PaymentPeriodType PaymentPeriodType { get; set; }

        public string PaymentId { get; set; }

        public string PayerId { get; set; }

        public string EditionDisplayName { get; set; }

        public long InvoiceNo { get; set; }

        public SubscriptionPaymentStatus Status { get; set; }

        public bool IsRecurring { get; set; }
        
        public string ExternalPaymentId { get; set; }

        public string SuccessUrl { get; set; }

        public string ErrorUrl { get; set; }
    }
}
