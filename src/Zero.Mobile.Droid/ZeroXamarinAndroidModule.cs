using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Zero
{
    [DependsOn(typeof(ZeroXamarinSharedModule))]
    public class ZeroXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZeroXamarinAndroidModule).GetAssembly());
        }
    }
}