using AML.TransactionTracker.Core.Exceptions;
using static AML.TransactionTracker.Core.Enums.EnumsCore;

namespace AML.TransactionTracker.Core.Entities
{
    public sealed class Transaction
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public Currency TransactionCurrency { get; set; }
        public TransactionType Type { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool Flagged { get; set; }
        public bool Validated { get; set; }
        public string? FlaggedReason { get; set; }

        public Transaction(Guid id, decimal amount, Currency currency, TransactionType type,
        Guid customerId, DateTime date, string description, bool flagged = false, 
        bool validated = false, string flaggedReason = null): 
            this(id, amount, type, customerId, date, description, flagged, validated, flaggedReason)
        {         
            TransactionCurrency = currency;
        }

        private Transaction(Guid id, decimal amount, TransactionType type,
        Guid customerId, DateTime date, string description, bool flagged = false, 
        bool validated = false, string flaggedReason = null)
        {
            Validate(id, date);

            Id = id;
            Amount = amount;
            Type = type;
            CustomerId = customerId;
            Date = date;
            Description = description;
            Flagged = flagged;
            Validated = validated;
            FlaggedReason = flaggedReason;
        }

        public Transaction(decimal amount, Currency currency, TransactionType type,
            Guid customerId, DateTime date, string description, bool flagged = false, bool validated = false, string flaggedReason = null)
                : this(Guid.NewGuid(), amount, currency, type, customerId, date, description, flagged, validated, flaggedReason)
        { }

        private void Validate(Guid id, DateTime date)
        {
            ValidateId(id);
            ValidateDate(date);
        }

        private void ValidateId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new InvalidEntityIdException(id);
            }
        }

        private void ValidateDate(DateTime date)
        {
            if (date > DateTime.Now)
            {
                throw new InvalidDateException($"Type {this.GetType()}: Transaction date should not be in the future {date}");
            }
        }
    }
}
