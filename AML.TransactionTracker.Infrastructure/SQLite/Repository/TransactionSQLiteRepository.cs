using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using RulesEngine.Models;

namespace AML.TransactionTracker.Infrastructure.SQLite.Repository
{
    public sealed class TransactionSQLiteRepository : ITransactionRepository
    {
        private readonly SQLiteContext _context;

        public TransactionSQLiteRepository(SQLiteContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await GetAsync(id);
            if(item != null)
            {
                _context.Transactions.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
            => await _context.Transactions.AnyAsync(x => x.Id == id);

        public async Task FlagTransactionAsync(Guid id)
        {
            var item = await GetAsync(id);
            if(item != null)
            {
                item.Flagged = true;
                _context.SaveChanges();
            }
        }

        public IQueryable<Transaction> GetAll()
            => _context.Transactions.AsQueryable();

        public async Task<Transaction?> GetAsync(Guid id)
            => await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Transaction>> GetFlaggedAsync()
        {
            var all = GetAll();
            var result = await all.Where(x => x.Flagged).ToListAsync();
            return result;
        }

        public async Task<int> GetNumberOfTransactionsSinceDate(DateTime date, Guid customerId)
        {
            var result = await _context.Transactions.Where(x => x.Date >= date && x.CustomerId == customerId).CountAsync();
            return result;
        }

        public async Task<IEnumerable<Workflow>> GetRulesForTransactionAsync(string name) => await _context.Workflows.Include(x => x.Rules).Where(x => x.WorkflowName == name).ToListAsync();

        public async Task UpdateAsync(Transaction transaction)
        {
            var item = await GetAsync(transaction.Id);

            if(item != null)
            {
                _context.Entry(item).CurrentValues.SetValues(transaction);
                await _context.SaveChangesAsync();
            }
        }
    }
}
