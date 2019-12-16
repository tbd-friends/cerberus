using System;
using System.Collections.Generic;
using MediatR;
using query.models;

namespace query.Requests
{
    public class GetAllCustomers : IRequest<IEnumerable<Customer>>
    {

    }

    public class GetCustomerById : IRequest<Customer>
    {
        public Guid Id { get; set; }
    }
}