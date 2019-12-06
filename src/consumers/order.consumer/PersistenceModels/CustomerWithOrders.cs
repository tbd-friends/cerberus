using System;
using System.Collections.Generic;

namespace order.consumer.PersistenceModels
{
    public class CustomerWithOrders
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Honorific { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }
}