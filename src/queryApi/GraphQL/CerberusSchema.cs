using System;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace queryApi.GraphQL
{
    public class CerberusSchema : Schema
    {
        public CerberusSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<CerberusQuery>();
        }
    }
}
