using AML.TransactionTracker.Core.Entities;
using MediatR;

namespace AML.TransactionTracker.Application.Commands
{
    public record AddCustomer : IRequest<Customer>
    {
        public string Name { get; init; }
        public string Surename { get; init; }
        public DateTime Birthdate { get; init; }
        public string Address { get; init; }

        public AddCustomer(string name, string surename, DateTime birthdate,
            string address)
        {
            Name = name;
            Surename = surename;
            Birthdate = birthdate;
            Address = address;
        }
    }
}
