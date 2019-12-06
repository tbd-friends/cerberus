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
        public void NewCustomer(CustomerInputModel model)
        {
            if (ModelState.IsValid)
            {
                _mediator.Send(new CreateNewCustomer
                { Honorific = model.Honorific, FirstName = model.FirstName, LastName = model.LastName });
            }
        }
    }
}