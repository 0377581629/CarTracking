using System;
using Abp.Application.Services.Dto;
using Abp.Timing;
using Zero.MultiTenancy.Payments;

namespace Zero.Sessions.Dto
{
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string ProfilePictureId { get; set; }
        
        #region Subscription
        public bool IsInTrialPeriod { get; set; }
        
        public DateTime? SubscriptionEndDateUtc { get; set; }
        
        public SubscriptionPaymentType SubscriptionPaymentType { get; set; }
        
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

        #endregion
    }
}
