using MediatR;

namespace AML.TransactionTracker.Application.Commands
{
    public record RemoveTransaction: IRequest
    {
        public Guid Id { get; init; }

        public RemoveTransaction(Guid id)
        {
            Id = id;
        }
    }
}
