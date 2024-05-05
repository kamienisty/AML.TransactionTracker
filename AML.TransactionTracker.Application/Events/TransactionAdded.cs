using AML.TransactionTracker.Core.Events;

namespace AML.TransactionTracker.Application.Events
{
    public record TransactionAdded: IEvent
    {
        public Guid TransactionId { get; init; }

        public TransactionAdded(Guid transactionId)
        {
            TransactionId = transactionId;
        }
    }
}
