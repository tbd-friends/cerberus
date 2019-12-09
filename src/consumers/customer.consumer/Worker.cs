using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using cerberus.core.kafka;
using consumer.models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
                    new TopicConsumer<Customer>("customers", _kafkaConfiguration)
                        .Start(OnCustomerMessage, stoppingToken)
                };

                await Task.WhenAll(topics);
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Topics could not be subscribed {exception}");
            }
        }

        private async Task<bool> OnCustomerMessage(Customer customer)
        {
            var context = _getStorage();

            var entry = await context.Get<Customer>(f => f.Id == customer.Id);

            if (entry != null && customer.IsDeleted)
            {
                await context.Delete<Customer>(f => f.Id == customer.Id);
            }
            else
            {
                entry = entry ?? new Customer() { Id = customer.Id };

                entry.FirstName = customer.FirstName;
                entry.LastName = customer.LastName;
                entry.Honorific = customer.Honorific;

                await context.Update(f => f.Id == customer.Id, entry);
            }

            return true;
        }
    }
}