using System;
using System.Collections.Generic;

namespace persistence.models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Honorific { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public static Customer Default(Guid id) => new Customer { Id = id, Addresses = new List<Address>() };
    }
}