using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using DPS.Lib.Application.Shared;
using DPS.Lib.Core;

namespace DPS.Lib.Application
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(typeof(LibApplicationSharedModule), typeof(LibCoreModule))]
    public class LibApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LibApplicationModule).GetAssembly());
        }
    }
}