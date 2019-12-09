using System;

namespace order.consumer.ConsumerModels
{
    public class CustomerOrder
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }

        public bool IsDeleted => CustomerId != Guid.Empty && Id != Guid.Empty && ItemId == Guid.Empty &&
                                 string.IsNullOrEmpty(ItemName) && Quantity == 0;
    }
}