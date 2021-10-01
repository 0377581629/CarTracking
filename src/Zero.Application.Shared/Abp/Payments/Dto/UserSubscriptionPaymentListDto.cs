using Abp.Application.Services.Dto;

namespace Zero.Abp.Payments.Dto
{
    public class UserSubscriptionPaymentListDto: AuditedEntityDto
    {
        public string Gateway { get; set; }

        public decimal Amount { get; set; }
        
        public string Currency { get; set; }

        public long UserId { get; set; }
        
        public string UserEmailAddress { get; set; }

        public int DayCount { get; set; }

        public string PaymentPeriodType { get; set; }

        public string ExternalPaymentId { get; set; }

        public string PayerId { get; set; }

        public string Status { get; set; }

        public string InvoiceNo { get; set; }

    }
}
