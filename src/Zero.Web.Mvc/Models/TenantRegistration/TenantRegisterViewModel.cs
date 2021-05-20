using Zero.Editions;
using Zero.Editions.Dto;
using Zero.MultiTenancy.Payments;
using Zero.Security;
using Zero.MultiTenancy.Payments.Dto;

namespace Zero.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public int? EditionId { get; set; }

        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
    }
}
