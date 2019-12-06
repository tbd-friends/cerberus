using System.Threading;
using System.Threading.Tasks;
using command.persistence.Context;
using command.persistence.Models;
using MediatR;
using messages.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace command.Handlers
{
    public class UpdateCustomerHandler : PublishingRequestHandler<UpdateCustomer, Customer>
    {
        private readonly ApplicationContext _context;

        public UpdateCustomerHandler(ApplicationContext context, IConfiguration configuration) : base("customers", configuration)
        {
            _context = context;
        }

        public override async Task<Unit> Handle(UpdateCustomer request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.SingleAsync(f => f.Id == request.Id, cancellationToken);

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Honorific = request.Honorific;

            await _context.SaveChangesAsync(cancellationToken);

            await Publish(customer);

            return Unit.Value;
        }
    }
}