using MediatR;
using static AML.TransactionTracker.Core.Enums.EnumsCore;

namespace AML.TransactionTracker.Application.Commands
{
    public record UpdateTransaction: IRequest
    {
        public Guid Id { get; init; }
        public decimal Amount { get; init; }
        public Currency TransactionCurrency { get;}
        public TransactionType Type { get; init; }
        public Guid CustomerId { get; init; }
        public DateTime Date { get; init; }
        public string Description { get; init; }

        public UpdateTransaction(Guid id, decimal amount, Currency currency, TransactionType type, 
            Guid customerId, DateTime date, string description)
        {
            Id = id;
            Amount = amount;
            TransactionCurrency = currency;
            Type = type;
            CustomerId = customerId;
            Date = date;
            Description = description;
        }
    }
}
