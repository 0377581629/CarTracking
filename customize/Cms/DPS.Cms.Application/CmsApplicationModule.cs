using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using DPS.Cms.Application.Shared;
using DPS.Cms.Core;

namespace DPS.Cms.Application
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(typeof(CmsApplicationSharedModule), typeof(CmsCoreModule))]
    public class CmsApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CmsApplicationModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            
        }
    }
}