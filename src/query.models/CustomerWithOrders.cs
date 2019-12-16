using System;
using System.Collections.Generic;

namespace query.models
{
    public class CustomerWithOrders
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Honorific { get; set; }
        public ICollection<CustomerOrder> Orders { get; set; }
    }

    public class CustomerOrder
    {
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }
}