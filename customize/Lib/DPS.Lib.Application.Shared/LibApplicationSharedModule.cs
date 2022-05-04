using Abp.Modules;
using Abp.Reflection.Extensions;
using DPS.Lib.Core.Shared;

namespace DPS.Lib.Application.Shared
{
    [DependsOn(typeof(LibCoreSharedModule))]
    public class LibApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LibApplicationSharedModule).GetAssembly());
        }
    }
}