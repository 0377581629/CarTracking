using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Zero.Configure;
using Zero.Startup;
using Zero.Test.Base;

namespace Zero.GraphQL.Tests
{
    [DependsOn(
        typeof(ZeroGraphQLModule),
        typeof(ZeroTestBaseModule))]
    public class ZeroGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZeroGraphQLTestModule).GetAssembly());
        }
    }
}