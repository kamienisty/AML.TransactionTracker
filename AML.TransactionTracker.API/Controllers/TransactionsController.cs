using AML.TransactionTracker.Application.Commands;
using AML.TransactionTracker.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AML.TransactionTracker.API.Controllers
{    
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : Controller
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IResult> Post(AddTransaction command)
        {
            var transaction = await _mediator.Send(command);

            return TypedResults.Created($"transaciton/{transaction.Id}");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IResult> Get(Guid id)
        {
            var command = new GetTransaction(id);
            var transaction = await _mediator.Send(command);

            return TypedResults.Ok(transaction);
        }

        [HttpPut]
        public async Task<IResult> Put(UpdateTransaction command)
        {
            await _mediator.Send(command);

            return TypedResults.NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IResult> Delete(Guid id)
        {
            var command = new RemoveTransaction(id);
            await _mediator.Send(command);

            return TypedResults.NoContent();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IResult> Flagged()
        {
            var result = await _mediator.Send(new GetFlaggedTransactions());

            return TypedResults.Ok(result);
        }

        [HttpPatch]
        [Route("{id}/flag")]
        public async Task<IResult> FlagTransaction(Guid id)
        {
            var command = new FlagTransaction(id);
            await _mediator.Send(command);

            return TypedResults.NoContent();
        }
    }
}
