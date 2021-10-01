using Abp.Domain.Services;

namespace Zero.Customize.BackgroundJobs
{
    public interface IUserSubscriptionBackgroundJob : IDomainService
    {
        void UserSubscriptionExpirationCheck();

        void UserSubscriptionExpireEmailNotifier();
    }
}