using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using cerberus.core.kafka;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using order.consumer.ConsumerModels;
using order.consumer.PersistenceModels;

namespace order.consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly GetContext _getContext;
        private readonly IDictionary<string, string> _kafkaConfiguration = new Dictionary<string, string>();

        public Worker(ILogger<Worker> logger, IConfiguration configuration, GetContext getContext)
        {
            _logger = logger;
            _getContext = getContext;

            configuration.Bind("kafka", _kafkaConfiguration);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var topics = new[]
            {
                new TopicConsumer<Customer>("customers", _kafkaConfiguration)
                    .Start(OnCustomerMessage, stoppingToken),
                new TopicConsumer<CustomerOrder>("orders", _kafkaConfiguration)
                    .Start(OnOrderMessage, stoppingToken)
        };

            await Task.WhenAll(topics);
        }

        private async Task<bool> OnOrderMessage(CustomerOrder order)
        {
            var context = _getContext();

            var entry = await context.Get<CustomerWithOrders>(f => f.Id == order.CustomerId) ??
                        new CustomerWithOrders() { Id = order.CustomerId, Orders = new List<Order>() };

            entry.Orders.Add(new Order
            {
                OrderId = order.Id,
                ItemId = order.ItemId,
                ItemName = order.ItemName,
                Quantity = order.Quantity
            });

            await context.Update(f => f.Id == order.CustomerId, entry);

            return true;
        }

        private async Task<bool> OnCustomerMessage(Customer customer)
        {
            var context = _getContext();

            var entry = await context.Get<CustomerWithOrders>(f => f.Id == customer.Id) ??
                        new CustomerWithOrders() { Id = customer.Id, Orders = new List<Order>() };

            entry.FirstName = customer.FirstName;
            entry.LastName = customer.LastName;
            entry.Honorific = customer.Honorific;

            await context.Update(f => f.Id == customer.Id, entry);

            return true;
        }
    }
}
