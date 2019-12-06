using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using command.persistence.Context;
using command.persistence.Models;
using Confluent.Kafka;
using MediatR;
using messages.Requests;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace command.Handlers
{
    public class CreateNewCustomerHandler : IRequestHandler<CreateNewCustomer>
    {
        private readonly ApplicationContext _context;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public CreateNewCustomerHandler(ApplicationContext context, IMediator mediator, IConfiguration configuration)
        {
            _context = context;
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(CreateNewCustomer request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Honorific = request.Honorific,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            await _context.AddAsync(customer, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            await PublishCustomer(customer);

            return Unit.Value;
        }

        private async Task PublishCustomer(Customer customer)
        {
            var kafkaConfiguration = new Dictionary<string, string>();

            _configuration.Bind("kafka", kafkaConfiguration);

            using (var producer = new ProducerBuilder<Null, Customer>(kafkaConfiguration)
                .SetValueSerializer(new KafkaJsonValueSerializer<Customer>()).Build())
            {
                await producer.ProduceAsync("customers", new Message<Null, Customer> { Value = customer });
            }
        }
    }

    public class KafkaJsonValueSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        }
    }
}