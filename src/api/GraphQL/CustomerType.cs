using GraphQL.Types;
using MediatR;
using query.models;
using query.Requests;

namespace api.GraphQL
{
    public sealed class CustomerType : ObjectGraphType<Customer>
    {
        public CustomerType(IMediator mediator)
        {
            Field(f => f.FirstName, false);
            Field(f => f.LastName, false);
            Field(f => f.Honorific, true);
            Field(f => f.Id, nullable: false);
            Field<ListGraphType<AddressType>>("addresses", resolve: context => context.Source.Addresses);
            Field<ListGraphType<CustomerOrderType>>("orders",
                resolve: context => mediator.Send(new GetCustomerWithOrders {Id = context.Source.Id}).GetAwaiter()
                    .GetResult().Orders);
        }
    }
}