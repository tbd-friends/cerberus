using System;
using MediatR;

namespace messages.Requests
{
    public class DeleteCustomerOrders : IRequest
    {
        public Guid Id { get; set; }
    }
}