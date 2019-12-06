using System.Collections.Generic;
using MediatR;
using query.models;

namespace query.Requests
{
    public class GetAllCustomers : IRequest<IEnumerable<Customer>>
    {

    }
}