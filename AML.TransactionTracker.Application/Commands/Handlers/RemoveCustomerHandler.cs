using AML.TransactionTracker.Core.Repositories;
using MediatR;

namespace AML.TransactionTracker.Application.Commands.Handlers
{
    public class RemoveCustomerHandler : HandlerBase<ICustomerRepository>, IRequestHandler<RemoveCustomer>
    {
        public RemoveCustomerHandler(ICustomerRepository repository) : base(repository)
        {
        }

        public async Task Handle(RemoveCustomer request, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsAsync(request.Id) && !await _repository.IsDeletedAsync(request.Id))
            {
                await _repository.DeleteAsync(request.Id);
            }
        }
    }
}
