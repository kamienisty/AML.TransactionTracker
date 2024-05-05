using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Repositories;
using MediatR;

namespace AML.TransactionTracker.Application.Commands.Handlers
{
    public class UpdateCustomerHandler : HandlerBase<ICustomerRepository>, IRequestHandler<UpdateCustomer>
    {
        public UpdateCustomerHandler(ICustomerRepository repository): base(repository)
        {

        }
        public async Task Handle(UpdateCustomer request, CancellationToken cancellationToken)
        {
            var customer = new Customer(
                request.Id,
                request.Name,
                request.Surename,
                request.Birthdate,
                request.Address);

            await _repository.UpdateAsync(customer);
        }
    }
}
