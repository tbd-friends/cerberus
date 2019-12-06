using System;

namespace command.persistence.Models
{
    public class CustomerOrder
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }
}