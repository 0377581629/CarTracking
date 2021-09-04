using Abp.Dependency;
using GraphQL.Types;
using GraphQL.Utilities;
using Zero.Queries.Container;
using System;

namespace Zero.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IServiceProvider provider) :
            base(provider)
        {
            Query = provider.GetRequiredService<QueryContainer>();
        }
    }
}