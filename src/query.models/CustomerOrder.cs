using System;

namespace query.models
{
    public class CustomerOrder
    {
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }
}