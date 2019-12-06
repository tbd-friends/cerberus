using System.Threading;
using System.Threading.Tasks;
using command.persistence.Context;
using MediatR;
using messages.Notifications;
using messages.Requests;
using Microsoft.EntityFrameworkCore;

namespace command.Handlers
{
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomer>
    {
        private readonly ApplicationContext _context;
        private readonly IMediator _mediator;

        public UpdateCustomerHandler(ApplicationContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateCustomer request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.SingleAsync(f => f.Id == request.Id, cancellationToken);

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Honorific = request.Honorific;

            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new CustomerUpdated
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Honorific = request.Honorific
            }, cancellationToken);

            return Unit.Value;
        }
    }
}