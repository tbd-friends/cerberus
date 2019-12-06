using System.Threading;
using System.Threading.Tasks;
using command.persistence.Context;
using command.persistence.Models;
using MediatR;
using messages.Requests;
using Microsoft.Extensions.Configuration;

namespace command.Handlers
{
    public class CreateNewCustomerHandler : PublishingRequestHandler<CreateNewCustomer, Customer>
    {
        private readonly ApplicationContext _context;

        public CreateNewCustomerHandler(ApplicationContext context, IConfiguration configuration)
            : base("customers", configuration)
        {
            _context = context;
        }

        public override async Task<Unit> Handle(CreateNewCustomer request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Honorific = request.Honorific,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            await _context.AddAsync(customer, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            await Publish(customer);

            return Unit.Value;
        }
    }
}