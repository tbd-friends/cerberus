using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using messages.Requests;
using query.models;
using query.persistence;
using query.Requests;

namespace query.Handlers
{
    public class GetAllCustomersHandler : IRequestHandler<GetAllCustomers, IEnumerable<Customer>>
    {
        private readonly ApplicationQueryContext _context;

        public GetAllCustomersHandler(ApplicationQueryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> Handle(GetAllCustomers request, CancellationToken cancellationToken)
        {
            return await _context.GetAll<Customer>();
        }
    }
}