using AML.TransactionTracker.Core.Entities;
using MediatR;

namespace AML.TransactionTracker.Application.Queries
{
    public record GetFlaggedTransactions: IRequest<IEnumerable<Transaction>>
    {

    }
}
