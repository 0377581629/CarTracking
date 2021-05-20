using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Zero
{
    public class ZeroClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZeroClientModule).GetAssembly());
        }
    }
}
