using Abp.Domain.Services;

namespace DPS.Lib.Application.Shared.BackgroundJobs
{
    public interface IWorkBackgroundJobs : IDomainService
    {
        void ApplyChangeWork();
        
        void ApplyChangeWorkChild();
    }
}