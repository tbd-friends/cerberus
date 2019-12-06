using System.Threading;
using System.Threading.Tasks;
using command.persistence.Context;
using command.persistence.Models;
using MediatR;
using messages.Requests;
using Microsoft.Extensions.Configuration;

namespace command.Handlers
{
    public class CreateCustomerOrderHandler : PublishingRequestHandler<CreateCustomerOrder, CustomerOrder>
    {
        private readonly ApplicationContext _context;

        public CreateCustomerOrderHandler(ApplicationContext context, IConfiguration configuration)
            : base("orders", configuration)
        {
            _context = context;
        }

        public override async Task<Unit> Handle(CreateCustomerOrder request, CancellationToken cancellationToken)
        {
            var order = new CustomerOrder
            {
                CustomerId = request.CustomerId,
                ItemId = request.ItemId,
                ItemName = request.ItemName,
                Quantity = request.Quantity
            };

            await _context.AddAsync(order, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            await Publish(order);

            return Unit.Value;
        }
    }
}