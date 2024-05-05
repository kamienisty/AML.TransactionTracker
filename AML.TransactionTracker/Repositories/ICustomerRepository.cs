using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Model;

namespace AML.TransactionTracker.Core.Repositories
{
    public interface ICustomerRepository: IRepository
    {
        Task<Customer?> GetAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Guid id);
        Task<bool> IsDeletedAsync(Guid id);
        IQueryable<Customer> GetAll();
        Task<IEnumerable<Customer>> Search(SearchModel search);
    }
}
