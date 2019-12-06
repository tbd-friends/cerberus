using System;
using System.Collections;
using System.Collections.Generic;

namespace query.persistence.Models
{
    public class CustomerOrders
    {
        public Guid Id { get; set; }
        public string Honorific { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<CustomerOrder> Orders { get; set; }
    }

    public class CustomerOrder
    {
        public Guid Id { get; set; }
        public decimal Value { get; set; }
    }
}