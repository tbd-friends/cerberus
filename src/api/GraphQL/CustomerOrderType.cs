using GraphQL.Types;
using query.models;

namespace api.GraphQL
{
    public sealed class CustomerOrderType : ObjectGraphType<CustomerOrder>
    {
        public CustomerOrderType()
        {
            Field(name: "name", f => f.ItemName);
            Field(name: "quantity", f => f.Quantity);
        }
    }
}