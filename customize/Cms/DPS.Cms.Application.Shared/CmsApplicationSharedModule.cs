using Abp.Modules;
using Abp.Reflection.Extensions;
using DPS.Cms.Core.Shared;

namespace DPS.Cms.Application.Shared
{
    [DependsOn(typeof(CmsCoreSharedModule))]
    public class CmsApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CmsApplicationSharedModule).GetAssembly());
        }
    }
}