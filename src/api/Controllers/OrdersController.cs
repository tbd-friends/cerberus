using System;
using System.Threading.Tasks;
using api.Input;
using MediatR;
using messages.Requests;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("{customerId:guid}")]
        public async Task CreateOrder(Guid customerId, NewCustomerOrderInputModel model)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new CreateCustomerOrder
                {
                    CustomerId = customerId,
                    ItemId = model.ItemId,
                    ItemName = model.ItemName,
                    Quantity = model.Quantity
                });
            }
        }
    }
}