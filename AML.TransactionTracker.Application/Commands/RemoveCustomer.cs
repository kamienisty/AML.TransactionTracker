using MediatR;

namespace AML.TransactionTracker.Application.Commands
{
    public record RemoveCustomer : IRequest
    {
        public Guid Id { get; init; }

        public RemoveCustomer(Guid id)
        {
            Id = id;
        }
    }
}
