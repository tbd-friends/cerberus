using System.Threading;
using System.Threading.Tasks;
using MediatR;
using messages.Notifications;
using MongoDB.Bson.IO;
using query.models;
using query.persistence;

namespace query.Projections
{
    public class CustomerDetailProjection : INotificationHandler<CustomerCreated>,
                                            INotificationHandler<CustomerUpdated>
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

        public async Task Handle(CustomerUpdated notification, CancellationToken cancellationToken)
        {
            await _context.Update(f => f.Id == notification.Id,
                new Customer()
                {
                    Id = notification.Id,
                    Honorific = notification.Honorific,
                    FirstName = notification.FirstName,
                    LastName = notification.LastName
                });
        }
    }
}