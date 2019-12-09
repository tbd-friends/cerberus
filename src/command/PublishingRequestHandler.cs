using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using cerberus.core.kafka;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace command
{
    public abstract class PublishingRequestHandler<TRequest, TPublished> : IRequestHandler<TRequest> where TRequest : IRequest<Unit>
    {
        private readonly string _topic;
        protected readonly IConfiguration _configuration;

        protected PublishingRequestHandler(string topic, IConfiguration configuration)
        {
            _topic = topic;
            _configuration = configuration;
        }

        public abstract Task<Unit> Handle(TRequest request, CancellationToken cancellationToken);

        public async Task Publish(TPublished published)
        {
            var kafkaConfiguration = new Dictionary<string, string>();

            _configuration.Bind("kafka", kafkaConfiguration);

            using (var producer = new ProducerBuilder<Null, TPublished>(kafkaConfiguration)
                .SetValueSerializer(new KafkaJsonValueSerializer<TPublished>()).Build())
            {
                await producer.ProduceAsync(_topic, new Message<Null, TPublished> { Value = published });
            }
        }
    }
}