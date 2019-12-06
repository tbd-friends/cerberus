using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using command.Notifications;
using MediatR;
using query.persistence;
using query.persistence.Models;

namespace query.Handlers
{
    public class CustomerOrdersProjection : INotificationHandler<CustomerCreated>
    {
        private readonly ApplicationQueryContext _context;

        public CustomerOrdersProjection(ApplicationQueryContext context)
        {
            _context = context;
        }

        public async Task Handle(CustomerCreated notification, CancellationToken cancellationToken)
        {
            await _context.Insert(new CustomerOrders
            {
                Id = notification.Id,
                FirstName = notification.FirstName,
                LastName = notification.LastName,
                Honorific = notification.Honorific,
                Orders = new List<CustomerOrder>()
            });
        }
    }
}