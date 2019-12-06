using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using cerberus.core.kafka;
using Confluent.Kafka;
using customer.consumer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace customer.consumer
{
    public class WorkerService : BackgroundService
    {
        private readonly IConsumer<Null, Customer> _consumer;
        private readonly GetContext _getContext;

        public WorkerService(IConfiguration configuration, GetContext getContext)
        {
            _getContext = getContext;

            var kafkaConfiguration = new Dictionary<string, string>();

            configuration.Bind("kafka", kafkaConfiguration);

            _consumer = new ConsumerBuilder<Null, Customer>(kafkaConfiguration)
                .SetValueDeserializer(new KafkaJsonValueDeserializer<Customer>())
                .Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe("customers");

            while (!stoppingToken.IsCancellationRequested)
            {
                var message = _consumer.Consume(stoppingToken);

                if (message != null)
                {
                    try
                    {
                        var context = _getContext();

                        await context.Update(f => f.Id == message.Value.Id, message.Value);

                        _consumer.Commit(message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }
    }
}