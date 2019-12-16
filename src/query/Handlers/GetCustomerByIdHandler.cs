using System.Threading;
using System.Threading.Tasks;
using MediatR;
using query.models;
using query.persistence;
using query.Requests;

namespace query.Handlers
{
    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerById, Customer>
    {
        private readonly ApplicationQueryContext _context;

        public GetCustomerByIdHandler(ApplicationQueryContext context)
        {
            _context = context;
        }

        public async Task<Customer> Handle(GetCustomerById request, CancellationToken cancellationToken)
        {
            return await _context.Get<Customer>(f => f.Id == request.Id);
        }
    }
}