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
    public class DeleteCustomerHandler : PublishingRequestHandler<DeleteCustomer, Customer>
    {
        private readonly ApplicationContext _context;
        private readonly IMediator _mediator;

        public DeleteCustomerHandler(IMediator mediator, ApplicationContext context, IConfiguration configuration)
            : base("customers", configuration)
        {
            _mediator = mediator;
            _context = context;
        }

        public override async Task<Unit> Handle(DeleteCustomer request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteCustomerOrders() { Id = request.Id }, cancellationToken);

            var customer = _context.Customers.Single(c => c.Id == request.Id);

            _context.Remove(customer);

            await _context.SaveChangesAsync(cancellationToken);

            await Publish(new Customer()
            {
                Id = request.Id
            });

            return Unit.Value;
        }
    }
}