using Abp.Modules;
using Abp.Reflection.Extensions;

namespace DPS.Lib.Core.Shared
{
    public class LibCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LibCoreSharedModule).GetAssembly());
        }
    }
}