using System;

namespace query.models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Honorific { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}