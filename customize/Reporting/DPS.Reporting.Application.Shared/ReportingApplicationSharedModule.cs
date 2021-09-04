using Abp.Modules;
using Abp.Reflection.Extensions;

namespace DPS.Reporting.Application.Shared
{
    public class ReportingApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ReportingApplicationSharedModule).GetAssembly());
        }
    }
}