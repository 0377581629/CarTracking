using System;
using Abp.Application.Services.Dto;
using Abp.Timing;
using Zero.MultiTenancy.Payments;

namespace Zero.Sessions.Dto
{
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }

        public Guid? LogoId { get; set; }

        public string LogoFileType { get; set; }

        public Guid? CustomCssId { get; set; }

        public DateTime? SubscriptionEndDateUtc { get; set; }

        public bool IsInTrialPeriod { get; set; }

        public SubscriptionPaymentType SubscriptionPaymentType { get; set; }

        public EditionInfoDto Edition { get; set; }

        public DateTime CreationTime { get; set; }

        public PaymentPeriodType PaymentPeriodType { get; set; }

        public string SubscriptionDateString { get; set; }

        public string CreationTimeString { get; set; }

        public bool IsInTrial()
        {
            return IsInTrialPeriod;
        }

        public bool SubscriptionIsExpiringSoon(int subscriptionExpireNootifyDayCount)
        {
            if (SubscriptionEndDateUtc.HasValue)
            {
                return Clock.Now.ToUniversalTime().AddDays(subscriptionExpireNootifyDayCount) >= SubscriptionEndDateUtc.Value;
            }

            return false;
        }

        public int GetSubscriptionExpiringDayCount()
        {
            if (!SubscriptionEndDateUtc.HasValue)
            {
                return 0;
            }

            return Convert.ToInt32(SubscriptionEndDateUtc.Value.ToUniversalTime().Subtract(Clock.Now.ToUniversalTime()).TotalDays);
        }

        public bool HasRecurringSubscription()
        {
            return SubscriptionPaymentType != SubscriptionPaymentType.Manual;
        }
        
        #region Extent

        public Guid? LoginLogoId { get; set; }

        public Guid? MenuLogoId { get; set; }

        public Guid? LoginBackgroundId { get; set; }

        public string LoginLogoFileType { get; set; }

        public string MenuLogoFileType { get; set; }

        public string LoginBackgroundFileType { get; set; }
        
        public string WebTitle { get; set; }

        public string WebDescription { get; set; }

        public string WebAuthor { get; set; }

        public string WebKeyword { get; set; }

        public string WebFavicon { get; set; }

        #endregion
        
        #region Subscription User - Depend on settings
        
        public bool UseSubscriptionUser { get; set; }
        
        #endregion
    }
}