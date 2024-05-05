using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Repositories;
using MediatR;

namespace AML.TransactionTracker.Application.Queries.Handlers
{
    public class GetCustomerHandler : HandlerBase<ICustomerRepository>, IRequestHandler<GetCustomer, Customer>
    {
        public GetCustomerHandler(ICustomerRepository repository): base(repository)
        {

        }
        public async Task<Customer> Handle(GetCustomer request, CancellationToken cancellationToken) 
            => await _repository.GetAsync(request.Id);
    }
}
