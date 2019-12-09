using System;
using MediatR;

namespace messages.Requests
{
    public class DeleteCustomer : IRequest
    {
        public Guid Id { get; set; }
    }
}