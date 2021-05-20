using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Zero
{
    public class ZeroCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZeroCoreSharedModule).GetAssembly());
        }
    }
}