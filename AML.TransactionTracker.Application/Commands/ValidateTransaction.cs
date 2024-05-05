using MediatR;

namespace AML.TransactionTracker.Application.Commands
{
    public record ValidateTransaction: IRequest
    {
        public Guid TransactionId { get; init; }

        public ValidateTransaction(Guid transactionId)
        {
            TransactionId = transactionId;
        }
    }
}
