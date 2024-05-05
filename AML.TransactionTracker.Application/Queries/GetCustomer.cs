using AML.TransactionTracker.Core.Entities;
using MediatR;

namespace AML.TransactionTracker.Application.Queries
{
    public record GetCustomer : IRequest<Customer>
    {
        public Guid Id { get; init; }

        public GetCustomer(Guid id)
        {
            Id = id;
        }
    }
}
