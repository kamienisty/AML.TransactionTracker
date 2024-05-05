using MediatR;

namespace AML.TransactionTracker.Application.Commands
{
    public record UpdateCustomer: IRequest
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Surename { get; init; }
        public DateTime Birthdate { get; init; }
        public string Address { get; init; }

        public UpdateCustomer(Guid id, string name, string surename, DateTime birthdate, string address)
        {
            Id = id;
            Name = name;
            Surename = surename;
            Birthdate = birthdate;
            Address = address;
        }
    }
}
