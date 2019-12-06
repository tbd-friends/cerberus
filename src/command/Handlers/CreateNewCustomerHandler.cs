using System.Threading;
using System.Threading.Tasks;
using command.Notifications;
using command.persistence.Context;
using command.persistence.Models;
using command.Requests;
using MediatR;

namespace command.Handlers
{
    public class CreateNewCustomerHandler : IRequestHandler<CreateNewCustomer>
    {
        private readonly ApplicationContext _context;
        private readonly IMediator _mediator;

        public CreateNewCustomerHandler(ApplicationContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
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

            await _mediator.Publish(new CustomerCreated
            {
                Id = customer.Id,
                Honorific = customer.Honorific,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            }, cancellationToken);

            return Unit.Value;
        }
    }
}