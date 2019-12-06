using System.Threading;
using System.Threading.Tasks;
using MediatR;
using query.models;
using query.persistence;
using query.Requests;

namespace query.Handlers
{
    public class GetCustomerWithOrdersHandler : IRequestHandler<GetCustomerWithOrders, CustomerWithOrders>
    {
        private readonly ApplicationQueryContext _context;

        public GetCustomerWithOrdersHandler(ApplicationQueryContext context)
        {
            _context = context;
        }

        public async Task<CustomerWithOrders> Handle(GetCustomerWithOrders request, CancellationToken cancellationToken)
        {
            return await _context.Get<CustomerWithOrders>(f => f.Id == request.Id);
        }
    }
}