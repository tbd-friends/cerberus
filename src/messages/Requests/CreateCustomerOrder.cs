using System;
using System.Dynamic;
using MediatR;

namespace messages.Requests
{
    public class CreateCustomerOrder : IRequest
    {
        public Guid CustomerId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }
}