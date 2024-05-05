using AML.TransactionTracker.Core.Entities;
using RulesEngine.Models;

namespace AML.TransactionTracker.Core.Repositories
{
    public interface ITransactionRepository: IRepository
    {
        Task<Transaction?> GetAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(Guid id);
        IQueryable<Transaction> GetAll();
        Task<IEnumerable<Transaction>> GetFlaggedAsync();
        Task FlagTransactionAsync(Guid id);
        Task<IEnumerable<Workflow>> GetRulesForTransactionAsync(string name);
        Task<int> GetNumberOfTransactionsSinceDate(DateTime date, Guid customerId);
    }
}
