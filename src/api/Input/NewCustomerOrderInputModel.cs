using System;

namespace api.Input
{
    public class NewCustomerOrderInputModel
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }
}