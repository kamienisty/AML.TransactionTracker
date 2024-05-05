using AML.TransactionTracker.Core.Repositories;
using MediatR;

namespace AML.TransactionTracker.Application.Commands.Handlers
{
    public class FlagTransactionHandler: HandlerBase<ITransactionRepository>, IRequestHandler<FlagTransaction>
    {
        public FlagTransactionHandler(ITransactionRepository repository): base(repository)
        {

        }

        public async Task Handle(FlagTransaction request, CancellationToken cancellationToken)
        {
            await _repository.FlagTransactionAsync(request.Id);
        }
    }
}
