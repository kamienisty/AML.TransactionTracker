using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AML.TransactionTracker.Application.Commands.Handlers
{
    public class UpdateTransactionHandler: HandlerBase<ITransactionRepository>, IRequestHandler<UpdateTransaction>
    {
        public UpdateTransactionHandler(ITransactionRepository repository): base(repository)
        {

        }

        public async Task Handle(UpdateTransaction request, CancellationToken cancellationToken)
        {
            var transactin = new Transaction(
                request.Id,
                request.Amount,
                request.TransactionCurrency,
                request.Type,
                request.CustomerId,
                request.Date,
                request.Description);

            await _repository.UpdateAsync(transactin);
        }
    }
}
