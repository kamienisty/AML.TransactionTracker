using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Model;
using AML.TransactionTracker.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AML.TransactionTracker.Infrastructure.SQLite.Repository
{
    public sealed class CustomerSQLiteRepository : ICustomerRepository
    {
        private readonly SQLiteContext _context;
        public CustomerSQLiteRepository(SQLiteContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await GetAsync(id);
            if (item != null)
            {
                item.Deleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
            => await _context.Customers.AnyAsync(x => x.Id == id);

        public IQueryable<Customer> GetAll() 
            => _context.Customers.Where(x => !x.Deleted).AsQueryable();

        public async Task<Customer?> GetAsync(Guid id)
            => await _context.Customers
            .Include(x => x.Transactions)
            .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);

        public async Task<bool> IsDeletedAsync(Guid id)
            => await _context.Customers.AnyAsync(x => x.Id == id && x.Deleted);

        public async Task UpdateAsync(Customer customer)
        {
            var item = await GetAsync(customer.Id);
            if(item != null)
            {
                _context.Entry(item).CurrentValues.SetValues(customer);
                await _context.SaveChangesAsync();
            }                
        }

        public async Task<IEnumerable<Customer>> Search(SearchModel search)
        {
            var query = GetAll();

            if (!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
            }

            if (!string.IsNullOrEmpty(search.Surename))
            {
                query = query.Where(x => x.Surename.Contains(search.Surename));
            }

            if(search.Birthdate != null)
            {
                query = query.Where(x => x.Birthdate == search.Birthdate);
            }

            if (!string.IsNullOrEmpty(search.Address))
            {
                query = query.Where(x => x.Address.Contains(search.Address));
            }

            return await query.ToListAsync();
        }
    }
}
