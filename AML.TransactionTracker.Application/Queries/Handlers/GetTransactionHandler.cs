using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AML.TransactionTracker.Application.Queries.Handlers
{
    public class GetTransactionHandler: HandlerBase<ITransactionRepository>, IRequestHandler<GetTransaction, Transaction>
    {
        public Guid Id { get; }

        public GetTransactionHandler(ITransactionRepository repository): base(repository)
        {

        }

        public async Task<Transaction> Handle(GetTransaction request, CancellationToken cancellationToken) 
            => await _repository.GetAsync(request.Id);
    }
}
