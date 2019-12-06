using System.Threading.Tasks;
using api.Input.Models;
using command.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task NewCustomer(CustomerInputModel model)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new CreateNewCustomer
                { Honorific = model.Honorific, FirstName = model.FirstName, LastName = model.LastName });
            }
        }
    }
}