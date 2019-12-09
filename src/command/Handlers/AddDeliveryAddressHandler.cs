using System.Threading;
using System.Threading.Tasks;
using command.persistence.Context;
using command.persistence.Models;
using command.Publishing;
using MediatR;
using messages.Requests;
using Microsoft.Extensions.Configuration;

namespace command.Handlers
{
    public class AddDeliveryAddressHandler : PublishingRequestHandler<AddDeliveryAddress, PublishedCustomerAddress>
    {
        private readonly ApplicationContext _context;

        public AddDeliveryAddressHandler(ApplicationContext context, IConfiguration configuration)
            : base("addresses", configuration)
        {
            _context = context;
        }

        public override async Task<Unit> Handle(AddDeliveryAddress request, CancellationToken cancellationToken)
        {
            var address = new Address
            {
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                AddressLine3 = request.AddressLine3,
                City = request.City,
                State = request.State,
                PostalCode = request.PostalCode
            };

            await _context.AddAsync(address, cancellationToken);

            var customerAddress = new CustomerAddress
            {
                CustomerId = request.CustomerId,
                AddressId = address.Id,
                IsDelivery = true
            };

            await _context.AddAsync(customerAddress, cancellationToken);

            await Publish(new PublishedCustomerAddress
            {
                CustomerId = request.CustomerId,
                AddressId = address.Id,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode
            });

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}