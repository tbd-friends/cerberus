using System;

namespace command.persistence.Models
{
    public class CustomerAddress
    {
        public Guid CustomerId { get; set; }
        public Guid AddressId { get; set; }
        public bool IsBilling { get; set; }
        public bool IsDelivery { get; set; }
    }
}