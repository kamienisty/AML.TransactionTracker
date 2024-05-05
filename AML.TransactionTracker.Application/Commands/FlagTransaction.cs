using MediatR;

namespace AML.TransactionTracker.Application.Commands
{
    public record FlagTransaction: IRequest
    {
        public Guid Id { get; init; }

        public FlagTransaction(Guid id)
        {
            Id = id;
        }
    }
}
