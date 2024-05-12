using AML.TransactionTracker.Core.Exceptions;

namespace AML.TransactionTracker.Core.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surename { get; set; }
        public DateTime Birthdate { get; set; }
        public string Address { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletionDate { get; set; }

        public virtual IEnumerable<Transaction> Transactions { get; set; }

        public Customer(Guid id, string name, string surename, DateTime birthdate,
            string address, bool deleted = false, DateTime? deletionDate = null, IEnumerable<Transaction> transactions = null)
        {
            Validate(id, birthdate);

            Id = id;
            Name = name;
            Surename = surename;
            Birthdate = birthdate;
            Address = address;
            Deleted = deleted;
            DeletionDate = deletionDate;

            Transactions = transactions ?? new List<Transaction>();
        }

        public Customer(string name, string surename, DateTime birthdate,
            string address, IEnumerable<Transaction> transactions = null) :
                this(Guid.NewGuid(), name, surename, birthdate, address, false, null, transactions)
        {
        }

        public Customer(string name, string surename, DateTime birthdate, string address) :
            this(name, surename, birthdate, address, null)
        {
        }

        private void Validate(Guid id, DateTime date)
        {
            ValidateId(id);
            ValidateBirthdate(date);
        }

        private void ValidateId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new InvalidEntityIdException(id);
            }
        }

        private void ValidateBirthdate(DateTime date)
        {
            if (date > DateTime.Now)
            {
                throw new InvalidDateException($"Type {this.GetType()}: Birthdate date should not be in the future {date}");
            }
        }


    }
}
