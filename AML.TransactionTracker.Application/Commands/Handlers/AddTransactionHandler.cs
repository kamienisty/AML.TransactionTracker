using AML.TransactionTracker.Application.Events;
using AML.TransactionTracker.Application.Exceptions;
using AML.TransactionTracker.Application.Services;
using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Repositories;
using MediatR;

namespace AML.TransactionTracker.Application.Commands.Handlers
{
    public class AddTransactionHandler: HandlerBase<ITransactionRepository>, IRequestHandler<AddTransaction, Transaction>
    {
        private readonly IEventPublisher _eventPublisher;

        public AddTransactionHandler(ITransactionRepository repository, IEventPublisher publisher): base(repository)
        {
            _eventPublisher = publisher;
        }

        public async Task<Transaction> Handle(AddTransaction request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction(
                request.Amount,
                request.TransactionCurrency,
                request.Type,
                request.CustomerId,
                request.Date,
                request.Description);

            if (await _repository.ExistsAsync(transaction.Id))
            {
                throw new EntityAlreadyExistsException(transaction.Id, this.GetType());
            }

            await _repository.AddAsync(transaction);

            try
            {
                await _eventPublisher.PublishAsync(new TransactionAdded(transaction.Id));
            }catch(Exception e)
            {


                throw;
            }

            return transaction;
        }
    }
}
