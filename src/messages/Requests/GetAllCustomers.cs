using System.Collections;
using System.Collections.Generic;
using MediatR;
using query.models;

namespace messages.Requests
{
    public class GetAllCustomers : IRequest<IEnumerable<Customer>>
    {
        
    }
}