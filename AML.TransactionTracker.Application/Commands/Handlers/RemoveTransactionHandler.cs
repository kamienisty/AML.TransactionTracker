using AML.TransactionTracker.Core.Repositories;
using MediatR;

namespace AML.TransactionTracker.Application.Commands.Handlers
{
    public class RemoveTransactionHandler: HandlerBase<ITransactionRepository>, IRequestHandler<RemoveTransaction>
    {
        public RemoveTransactionHandler(ITransactionRepository repository): base(repository)
        {

        }

        public async Task Handle(RemoveTransaction request, CancellationToken cancellationToken)
            => await _repository.DeleteAsync(request.Id);
    }
}
