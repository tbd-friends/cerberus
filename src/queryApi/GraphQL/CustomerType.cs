using GraphQL.Types;
using query.models;
using query.persistence;

namespace queryApi.GraphQL
{
    public sealed class CustomerType : ObjectGraphType<Customer>
    {
        public CustomerType(ApplicationQueryContext query)
        {
            Field(f => f.FirstName, false);
            Field(f => f.LastName, false);
            Field(f => f.Honorific, true);
            Field(f => f.Id, nullable: false);
            Field<ListGraphType<AddressType>>("addresses", resolve: context => context.Source.Addresses);
            Field<ListGraphType<CustomerOrderType>>("orders", resolve: context => context.Source.Orders);
        }
    }
}