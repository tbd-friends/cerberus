using System.Threading;
using System.Threading.Tasks;
using command.Requests;
using MediatR;

namespace command.Handlers
{
    public class CreateNewCustomerHandler : IRequestHandler<CreateNewCustomer>
    {
        public Task<Unit> Handle(CreateNewCustomer request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}