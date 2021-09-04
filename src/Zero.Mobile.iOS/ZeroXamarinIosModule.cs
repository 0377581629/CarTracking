using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Zero
{
    [DependsOn(typeof(ZeroXamarinSharedModule))]
    public class ZeroXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZeroXamarinIosModule).GetAssembly());
        }
    }
}