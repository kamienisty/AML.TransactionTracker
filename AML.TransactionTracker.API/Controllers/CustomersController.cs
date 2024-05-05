using Microsoft.AspNetCore.Mvc;
using MediatR;
using AML.TransactionTracker.Application.Commands;
using AML.TransactionTracker.Application.Queries;

namespace AML.TransactionTracker.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IResult> Post(AddCustomer command)
        {
            var customer = await _mediator.Send(command);

            return TypedResults.Created($"customers/{customer.Id}");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IResult> Get(Guid id)
        {
            var command = new GetCustomer(id);
            var customer = await _mediator.Send(command);

            return TypedResults.Ok(customer);
        }

        [HttpPut]
        public async Task<IResult> Put(UpdateCustomer command)
        {
            await _mediator.Send(command);

            return TypedResults.NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IResult> Delete(Guid id)
        {
            var command = new RemoveCustomer(id);
            await _mediator.Send(command);

            return TypedResults.NoContent();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IResult> Search(string? name, string? surename, DateTime? birthdate, string? address)
        {           
            var command = new SearchCustomer(name, surename, birthdate, address);
            var result = await _mediator.Send(command);

            return TypedResults.Ok(result);
        }
    }
}
