using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using customer.consumer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

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

                        await context.Insert(message.Value);

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

    public class KafkaJsonValueDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));
        }
    }
}