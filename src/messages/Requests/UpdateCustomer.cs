using System;
using MediatR;

namespace messages.Requests
{
    public class UpdateCustomer : IRequest
    {
        public Guid Id { get; set; }
        public string Honorific { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}