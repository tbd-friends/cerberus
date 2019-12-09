using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using command.persistence.Context;
using command.persistence.Models;
using MediatR;
using messages.Requests;
using Microsoft.Extensions.Configuration;

namespace command.Handlers
{
    public class DeleteCustomerOrdersHandler : PublishingRequestHandler<DeleteCustomerOrders, CustomerOrder>
    {
        private readonly ApplicationContext _context;

        public DeleteCustomerOrdersHandler(ApplicationContext context, IConfiguration configuration)
            : base("orders", configuration)
        {
            _context = context;
        }

        public override async Task<Unit> Handle(DeleteCustomerOrders request, CancellationToken cancellationToken)
        {
            var ordersForCustomer = _context.CustomerOrders.Where(o => o.CustomerId == request.Id);

            foreach (var order in ordersForCustomer)
            {
                _context.Remove(order);

                await Publish(new CustomerOrder
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId
                });
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}