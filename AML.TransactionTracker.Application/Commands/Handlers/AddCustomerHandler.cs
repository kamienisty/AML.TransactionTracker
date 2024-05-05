using AML.TransactionTracker.Application.Exceptions;
using AML.TransactionTracker.Core.Repositories;
using AML.TransactionTracker.Core.Entities;
using MediatR;

namespace AML.TransactionTracker.Application.Commands.Handlers
{
    public class AddCustomerHandler : HandlerBase<ICustomerRepository>, IRequestHandler<AddCustomer, Customer>
    {
        public AddCustomerHandler(ICustomerRepository repository) : base(repository)
        {
        }
        public async Task<Customer> Handle(AddCustomer request, CancellationToken cancellationToken)
        {
            var customer = new Customer(
                request.Name,
                request.Surename,
                request.Birthdate,
                request.Address);

            if (await _repository.ExistsAsync(customer.Id))
                throw new EntityAlreadyExistsException(customer.Id, this.GetType());

            await _repository.AddAsync(customer);

            return customer;
        }
    }
}
