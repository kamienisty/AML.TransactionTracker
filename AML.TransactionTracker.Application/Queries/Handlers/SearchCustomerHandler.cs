using AML.TransactionTracker.Application.Exceptions;
using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Model;
using AML.TransactionTracker.Core.Repositories;
using MediatR;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace AML.TransactionTracker.Application.Queries.Handlers
{
    public class SearchCustomerHandler : HandlerBase<ICustomerRepository>, IRequestHandler<SearchCustomer, IEnumerable<Customer>>
    {
        public SearchCustomerHandler(ICustomerRepository repository) : base(repository)
        {

        }
        public async Task<IEnumerable<Customer>> Handle(SearchCustomer request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name) && string.IsNullOrEmpty(request.Surename)
                && request.Birthdate == null && string.IsNullOrEmpty(request.Address))
            {
                throw new EmptyParametersException("Serach received no parameters.");
            }

            var searchModel = new SearchModel(request.Name, request.Surename, request.Birthdate, request.Address);

            return await _repository.Search(searchModel);
        }
    }
}
