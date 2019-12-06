using System;
using MediatR;
using query.models;

namespace query.Requests
{
    public class GetCustomerWithOrders : IRequest<CustomerWithOrders>
    {
        public Guid Id { get; set; }
    }
}