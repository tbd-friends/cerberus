using System;
using MediatR;
using query.models;

namespace query.Requests
{
    public class GetCustomerWithOrders : IRequest<Customer>
    {
        public Guid Id { get; set; }
    }
}