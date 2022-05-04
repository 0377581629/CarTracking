using Abp.Modules;
using Abp.Reflection.Extensions;
using DPS.Lib.Core.Shared;

namespace DPS.Lib.Core
{
    [DependsOn(typeof(LibCoreSharedModule))]
    public class LibCoreModule: AbpModule
    {
        public override void PreInitialize()
        {
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LibCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
        }
    }
}