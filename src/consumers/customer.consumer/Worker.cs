using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using cerberus.core.kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using persistence.models;
using CustomerMessage = consumer.models.Customer;
using AddressMessage = consumer.models.CustomerAddress;
using Customer = persistence.models.Customer;
using CustomerOrderMessage = consumer.models.CustomerOrder;

namespace customer.consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly GetStorage _getStorage;
        private readonly IDictionary<string, string> _kafkaConfiguration = new Dictionary<string, string>();

        public Worker(ILogger<Worker> logger, IConfiguration configuration, GetStorage getStorage)
        {
            _logger = logger;
            _getStorage = getStorage;

            configuration.Bind("kafka", _kafkaConfiguration);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var topics = new[]
                {
                    new TopicConsumer<CustomerMessage>("customers", _kafkaConfiguration)
                        .Start(OnCustomerMessage, stoppingToken),
                    new TopicConsumer<AddressMessage>("addresses", _kafkaConfiguration)
                        .Start(OnAddressMessage, stoppingToken),
                    new TopicConsumer<CustomerOrderMessage>("orders", _kafkaConfiguration)
                        .Start(OnOrderMessage, stoppingToken)
                };

                await Task.WhenAll(topics);
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Topics could not be subscribed {exception}");
            }
        }

        private async Task<bool> OnOrderMessage(CustomerOrderMessage order)
        {
            var context = _getStorage();

            var entry = await context.Get<Customer>(f => f.Id == order.CustomerId) ??
                        (!order.IsDeleted
                            ? Customer.Default(order.CustomerId)
                            : null);

            if (entry != null)
            {
                if (!order.IsDeleted)
                {
                    entry.Orders.Add(new Order
                    {
                        OrderId = order.Id,
                        ItemId = order.ItemId,
                        ItemName = order.ItemName,
                        Quantity = order.Quantity
                    });
                }
                else
                {
                    entry.Orders.Remove(entry.Orders.Single(o => o.OrderId == order.Id));
                }

                await context.Update(f => f.Id == order.CustomerId, entry);
            }

            return true;
        }

        private async Task<bool> OnAddressMessage(AddressMessage address)
        {
            var storage = _getStorage();

            var entry = await storage.Get<Customer>(f => f.Id == address.CustomerId) ??
                        Customer.Default(address.CustomerId);

            entry.Addresses = entry.Addresses ?? new List<Address>();

            entry.Addresses.Add(new Address
            {
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode
            });

            await storage.Update(f => f.Id == address.CustomerId, entry);

            return true;
        }

        private async Task<bool> OnCustomerMessage(CustomerMessage customer)
        {
            var storage = _getStorage();

            var entry = await storage.Get<Customer>(f => f.Id == customer.Id);

            if (entry != null && customer.IsDeleted)
            {
                await storage.Delete<Customer>(f => f.Id == customer.Id);
            }
            else
            {
                entry = entry ?? Customer.Default(customer.Id);

                entry.FirstName = customer.FirstName;
                entry.LastName = customer.LastName;
                entry.Honorific = customer.Honorific;

                await storage.Update(f => f.Id == customer.Id, entry);
            }

            return true;
        }
    }
}