using System.Threading;
using System.Threading.Tasks;
using command.Notifications;
using MediatR;
using query.persistence;
using query.persistence.Models;

namespace query.Handlers
{
    public class CustomerDetailProjection : INotificationHandler<CustomerCreated>
    {
        private readonly ApplicationQueryContext _context;

        public CustomerDetailProjection(ApplicationQueryContext context)
        {
            _context = context;
        }

        public async Task Handle(CustomerCreated notification, CancellationToken cancellationToken)
        {
            await _context.Insert(new Customer
            {
                Id = notification.Id,
                Honorific = notification.Honorific,
                FirstName = notification.FirstName,
                LastName = notification.LastName
            });
        }
    }
}