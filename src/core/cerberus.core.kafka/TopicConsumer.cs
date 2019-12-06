using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace cerberus.core.kafka
{


    public class TopicConsumer<T>
    {
        private readonly IDictionary<string, string> _configuration;
        private readonly string _topic;

        public TopicConsumer(string topic, IDictionary<string, string> configuration)
        {
            _topic = topic;
            _configuration = configuration;
        }

        public Task Start(Func<T, Task<bool>> onMessageConsumed, CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                var consumer = new ConsumerBuilder<Null, T>(_configuration)
                    .SetValueDeserializer(new KafkaJsonValueDeserializer<T>())
                    .Build();

                consumer.Subscribe(_topic);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var message = consumer.Consume(stoppingToken);

                        if (message != null)
                        {
                            if (await onMessageConsumed(message.Value))
                            {
                                consumer.Commit(message);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                }
            }, stoppingToken);
        }
    }
}