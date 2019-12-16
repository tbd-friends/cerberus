using System.Threading;
using System.Threading.Tasks;
using MediatR;
using query.models;
using query.persistence;
using query.Requests;

namespace query.Handlers
{
    public class GetCustomerWithOrdersHandler : IRequestHandler<GetCustomerWithOrders, Customer>
    {
        private readonly ApplicationQueryContext _context;

        public GetCustomerWithOrdersHandler(ApplicationQueryContext context)
        {
            _context = context;
        }

        public async Task<Customer> Handle(GetCustomerWithOrders request, CancellationToken cancellationToken)
        {
            return await _context.Get<Customer>(f => f.Id == request.Id);
        }
    }
}