using AML.TransactionTracker.Core.Entities;
using MediatR;

namespace AML.TransactionTracker.Application.Queries
{
    public record GetTransaction: IRequest<Transaction>
    {
        public Guid Id { get; init; }

        public GetTransaction(Guid id)
        {
            Id = id;
        }
    }
}
