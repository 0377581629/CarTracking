using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using DPS.Reporting.Application.Shared;

namespace DPS.Reporting.Application
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(ReportingApplicationSharedModule)
        )]
    public class ReportingApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ReportingApplicationModule).GetAssembly());
        }
    }
}