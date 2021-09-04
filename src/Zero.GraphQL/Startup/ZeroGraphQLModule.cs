using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Zero.Startup
{
    [DependsOn(typeof(ZeroCoreModule))]
    public class ZeroGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZeroGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}