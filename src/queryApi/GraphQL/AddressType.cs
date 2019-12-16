using GraphQL.Types;
using query.models;

namespace queryApi.GraphQL
{
    public sealed class AddressType : ObjectGraphType<CustomerAddress>
    {
        public AddressType()
        {
            Field(f => f.Id, false);
            Field(f => f.AddressLine1, false);
        }
    }
}