using System;
using command.persistence.Context;
using GraphQL.Types;
using MediatR;
using query.models;
using query.Requests;

namespace api.GraphQL
{
    public class CerberusQuery : ObjectGraphType
    {
        public CerberusQuery(IMediator mediator)
        {
            Field<ListGraphType<CustomerType>>(
                "customers",
                resolve: context =>
                {
                    if (context.HasArgument("id"))
                    {
                        return new[]
                        {
                            mediator.Send(new GetCustomerById {Id = (Guid) context.Arguments["id"]}).GetAwaiter()
                                .GetResult()
                        };
                    }

                    return mediator.Send(new GetAllCustomers()).GetAwaiter().GetResult();
                },
                arguments: new QueryArguments(
                    new QueryArgument<GuidGraphType>() { Name = "id", DefaultValue = null }));
        }
    }
}
