using Abp.Modules;
using Abp.Reflection.Extensions;

namespace DPS.Cms.Core.Shared
{
    public class CmsCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CmsCoreSharedModule).GetAssembly());
        }
    }
}