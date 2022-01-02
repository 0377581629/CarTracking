using Abp.Modules;
using Abp.Reflection.Extensions;
using DPS.Cms.Core.Shared;

namespace DPS.Cms.Core
{
    [DependsOn(typeof(CmsCoreSharedModule))]
    public class CmsCoreModule: AbpModule
    {
        public override void PreInitialize()
        {
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CmsCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
        }
    }
}