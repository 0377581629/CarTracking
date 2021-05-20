using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Zero
{
    [DependsOn(typeof(ZeroCoreSharedModule))]
    public class ZeroApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZeroApplicationSharedModule).GetAssembly());
        }
    }
}