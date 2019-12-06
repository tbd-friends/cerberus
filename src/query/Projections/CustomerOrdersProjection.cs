using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using messages.Notifications;
using query.models;
using query.persistence;

namespace query.Projections
{
    public class CustomerOrdersProjection : INotificationHandler<CustomerCreated>, 
                                            INotificationHandler<CustomerUpdated>
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

        public async Task Handle(CustomerUpdated notification, CancellationToken cancellationToken)
        {
            await _context.Update(f => f.Id == notification.Id,
                new CustomerOrders
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