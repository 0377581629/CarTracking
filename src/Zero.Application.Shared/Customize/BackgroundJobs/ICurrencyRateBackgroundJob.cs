using Abp.Domain.Services;

namespace Zero.Customize.BackgroundJobs
{
    public interface ICurrencyRateBackgroundJob : IDomainService
    {
        void UpdateRates();
    }
}