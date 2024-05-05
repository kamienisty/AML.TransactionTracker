using AML.TransactionTracker.Core.Entities;
using static AML.TransactionTracker.Core.Enums.EnumsCore;
using MediatR;

namespace AML.TransactionTracker.Application.Commands
{
    public record AddTransaction: IRequest<Transaction>
    {
        public decimal Amount { get; init; }
        public Currency TransactionCurrency { get; init; }
        public TransactionType Type { get; init; }
        public Guid CustomerId { get; init; }
        public DateTime Date { get; init; }
        public string Description { get; init; }

        public AddTransaction(decimal amount, Currency transactionCurrency, TransactionType type, 
            Guid customerId, DateTime date, string description)
        {
            Amount = amount;
            TransactionCurrency = transactionCurrency;
            Type = type;
            CustomerId = customerId;
            Date = date;
            Description = description;
        }
    }
}
