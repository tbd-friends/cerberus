using System;
using GraphQL.Types;
using query.models;
using query.persistence;

namespace queryApi.GraphQL
{
    public class CerberusQuery : ObjectGraphType
    {
        public CerberusQuery(ApplicationQueryContext query)
        {
            Field<ListGraphType<CustomerType>>(
                "customers",
                resolve: context =>
                {
                    if (context.HasArgument("id"))
                    {
                        return new[]
                        {
                            query.Get<Customer>(c => c.Id == context.GetArgument("id", Guid.Empty)).GetAwaiter()
                                .GetResult()
                        };
                    }

                    return query.GetAll<Customer>();
                },
                arguments: new QueryArguments(
                    new QueryArgument<GuidGraphType>() { Name = "id", DefaultValue = null }));
        }
    }
}
