﻿using System;

namespace order.consumer.ConsumerModels
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