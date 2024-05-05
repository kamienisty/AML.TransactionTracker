using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Repositories;
using MediatR;

namespace AML.TransactionTracker.Application.Queries.Handlers
{
    public class GetFlaggedTransactionsHandler : HandlerBase<ITransactionRepository>, IRequestHandler<GetFlaggedTransactions, IEnumerable<Transaction>>
    {
        public GetFlaggedTransactionsHandler(ITransactionRepository repository): base(repository)
        {

        }
        public async Task<IEnumerable<Transaction>> Handle(GetFlaggedTransactions request, CancellationToken cancellationToken) 
            => await _repository.GetFlaggedAsync();
    }
}
