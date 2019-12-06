using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Input.Models;
using MediatR;
using messages.Requests;
using Microsoft.AspNetCore.Mvc;
using query.models;
using query.Requests;

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

        [HttpGet]
        public async Task<IEnumerable<Customer>> CustomerList()
        {
            return await _mediator.Send(new GetAllCustomers());
        }

        [HttpPut, Route("{id:guid}")]
        public async Task UpdateCustomer(Guid id, CustomerInputModel model)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateCustomer()
                {
                    Id = id,
                    Honorific = model.Honorific,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                });
            }
        }

        [HttpGet, Route("{id:guid}/orders")]
        public async Task<CustomerWithOrders> GetCustomerWithOrders(Guid id)
        {
            return await _mediator.Send(new GetCustomerWithOrders {Id = id});
        }
    }
}