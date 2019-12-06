using System.Threading;
using System.Threading.Tasks;
using command.persistence.Context;
using command.persistence.Models;
using command.Requests;
using MediatR;

namespace command.Handlers
{
    public class CreateNewCustomerHandler : IRequestHandler<CreateNewCustomer>
    {
        private readonly ApplicationContext _context;

        public CreateNewCustomerHandler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateNewCustomer request, CancellationToken cancellationToken)
        {
            await _context.AddAsync(new Customer
            {
                Honorific = request.Honorific,
                FirstName = request.FirstName,
                LastName = request.LastName
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}