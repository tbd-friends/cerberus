using System;

namespace command.Publishing
{
    public class PublishedCustomerAddress
    {
        public Guid CustomerId { get; set; }
        public Guid AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }
}