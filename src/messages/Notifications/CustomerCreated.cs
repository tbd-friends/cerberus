using System;
using MediatR;

namespace command.Notifications
{
    public class CustomerCreated : INotification
    {
        public Guid Id { get; set; }
        public string Honorific { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}