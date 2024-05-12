using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Model;
using AML.TransactionTracker.Infrastructure.SQLite;
using AML.TransactionTracker.Infrastructure.SQLite.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using static AML.TransactionTracker.Core.Enums.EnumsCore;

namespace AML.TransactionTracker.Infrastructure.Tests.Repositories
{
    public class CustomerSQLiteRepositoryTest
    {
        private Mock<SQLiteContext> _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptions<SQLiteContext>();
            _context = new Mock<SQLiteContext>(options);
        }

        [Test]
        public async Task AddAsync_CallsCorrectMethods_WhenCalled()
        {
            var mockSet = new Mock<DbSet<Customer>>();
            _context.Setup(x => x.Customers).Returns(mockSet.Object);

            var customer = new Customer("name", "surename", DateTime.Now, "address");
            var repo = new CustomerSQLiteRepository(_context.Object);

            await repo.AddAsync(customer);

            mockSet.Verify(x => x.AddAsync(It.IsAny<Customer>(), new CancellationToken()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetAsync_ReturnsCustomer_WhenCustomerIsNotDeleted()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var data = new List<Customer>
            {
                new Customer(id, "name", "surename", DateTime.Now, "address", false, null, null)
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Customers).Returns(mockSet.Object);
            var repo = new CustomerSQLiteRepository(_context.Object);

            var result = await repo.GetAsync(id);

            Assert.NotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [Test]
        public async Task GetAsync_ReturnsCustomerWithTransactions_WhenCustomerIsNotDeleted()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var data = new List<Customer>
            {
                new Customer(id, "name", "surename", DateTime.Now, "address", false, null,
                new List<Transaction>
                {
                    new Transaction(100, Currency.EURO, TransactionType.Deposit, id, DateTime.Now, "description")
                })
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Customers).Returns(mockSet.Object);
            var repo = new CustomerSQLiteRepository(_context.Object);

            var result = await repo.GetAsync(id);

            Assert.NotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.IsTrue(result.Transactions.Any(x => x.CustomerId == id));
            Assert.AreEqual(1, result.Transactions.Count());
        }

        [Test]
        public async Task GetAsync_ReturnsNull_WhenCustomerIsDeleted()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var data = new List<Customer>
            {
                new Customer(id, "name", "surename", DateTime.Now, "address", true, null, null)
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Customers).Returns(mockSet.Object);
            var repo = new CustomerSQLiteRepository(_context.Object);

            var result = await repo.GetAsync(id);

            Assert.IsNull(result);
        }

        [Test]
        public async Task DeleteAsync_MarksCustomerAsDeleted_WhenCorrectIdIsSend()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var data = new List<Customer>
            {
                new Customer(id, "name", "surename", DateTime.Now, "address", false, null, null)
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Customers).Returns(mockSet.Object);
            var repo = new CustomerSQLiteRepository(_context.Object);

            await repo.DeleteAsync(id);

            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsTrue(data.First().Deleted);
        }

        [Test]
        public async Task DeleteAsync_DoesNotSave_WhenThereIsNoCustomerWithId()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var data = new List<Customer>
            {
                new Customer(Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111"), "name", "surename", DateTime.Now, "address", false, null, null)
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Customers).Returns(mockSet.Object);
            var repo = new CustomerSQLiteRepository(_context.Object);

            await repo.DeleteAsync(id);

            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            Assert.IsFalse(data.First().Deleted);
        }

        [Test]
        public async Task ExistAsync_ReturnsTrue_WhenItemExists()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var data = new List<Customer>
            {
                new Customer(id, "name", "surename", DateTime.Now, "address", false, null, null)
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Customers).Returns(mockSet.Object);
            var repo = new CustomerSQLiteRepository(_context.Object);

            var result = await repo.ExistsAsync(id);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task ExistAsync_ReturnsFalse_WhenItemDoesNotExists()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var data = new List<Customer>
            {
                new Customer(Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111"), "name", "surename", DateTime.Now, "address", false, null, null)
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Customers).Returns(mockSet.Object);
            var repo = new CustomerSQLiteRepository(_context.Object);

            var result = await repo.ExistsAsync(id);

            Assert.IsFalse(result);
        }

        [Test]
        public void GetAll_ReturnsQueryableCollection_WhenCalled()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var data = new List<Customer>
            {
                new Customer(id, "name", "surename", DateTime.Now, "address", false, null, null)
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Customers).Returns(mockSet.Object);
            var repo = new CustomerSQLiteRepository(_context.Object);

            var result = repo.GetAll();

            Assert.IsTrue(result is IQueryable<Customer>);
            Assert.IsTrue(result.Any());
        }

        [Test]
        public async Task IsDeletedAsync_ReturnsTrue_WhenCustomerIsDeleted()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var data = new List<Customer>
            {
                new Customer(id, "name", "surename", DateTime.Now, "address", true, null, null)
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Customers).Returns(mockSet.Object);
            var repo = new CustomerSQLiteRepository(_context.Object);

            var result = await repo.IsDeletedAsync(id);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsDeletedAsync_Returnsfalse_WhenCustomerIsNotDeleted()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var data = new List<Customer>
            {
                new Customer(id, "name", "surename", DateTime.Now, "address", false, null, null)
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Customers).Returns(mockSet.Object);
            var repo = new CustomerSQLiteRepository(_context.Object);

            var result = await repo.IsDeletedAsync(id);

            Assert.IsFalse(result);
        }

        [TestCase("name1", null, null, null)]
        [TestCase(null, "surename1", null, null)]
        [TestCase(null, null, "1999-01-01", null)]
        [TestCase(null, null, null, "address1")]
        public async Task Search_ReturnsCustomers_ThatMeetSearchCriteria(string name, string surename, DateTime? date, string address)
        {
            var id1 = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111");
            var id2 = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0222");

            var data = new List<Customer>
            {
                new Customer(id1, "name1", "surename1", DateTime.Parse("1999-01-01"), "address1", false, null, null),
                new Customer(id2, "name2", "surename2", DateTime.Parse("2000-02-02"), "address2", false, null, null)
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Customers).Returns(mockSet.Object);
            var repo = new CustomerSQLiteRepository(_context.Object);
            var serachModel = new SearchModel(name, surename, date, address);

            var result = await repo.Search(serachModel);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(id1, result.First().Id);
        }

        [TestCase("name", null, null, null)]
        [TestCase(null, "surename", null, null)]
        [TestCase(null, null, null, "address")]
        public async Task Search_ReturnsCustomers_ThatMeetSearchPartialCriteria(string name, string surename, DateTime? date, string address)
        {
            var id1 = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111");
            var id2 = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0222");

            var data = new List<Customer>
            {
                new Customer(id1, "name1", "surename1", DateTime.Parse("1999-01-01"), "address1", false, null, null),
                new Customer(id2, "222", "222", DateTime.Parse("2000-02-02"), "2222", false, null, null)
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Customers).Returns(mockSet.Object);
            var repo = new CustomerSQLiteRepository(_context.Object);
            var serachModel = new SearchModel(name, surename, date, address);

            var result = await repo.Search(serachModel);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(id1, result.First().Id);
        }

        [TestCase("John", null, null, null)]
        [TestCase(null, "Doe", null, null)]
        [TestCase(null, null, "2001-03-03", null)]
        [TestCase(null, null, null, "street")]
        public async Task Search_ReturnsEmptyCollection_WhenNoOneMeetsCriteria(string name, string surename, DateTime? date, string address)
        {
            var id1 = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111");
            var id2 = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0222");

            var data = new List<Customer>
            {
                new Customer(id1, "name1", "surename1", DateTime.Parse("1999-01-01"), "address1", false, null, null),
                new Customer(id2, "222", "222", DateTime.Parse("2000-02-02"), "2222", false, null, null)
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Customers).Returns(mockSet.Object);
            var repo = new CustomerSQLiteRepository(_context.Object);
            var serachModel = new SearchModel(name, surename, date, address);

            var result = await repo.Search(serachModel);

            Assert.AreEqual(0, result.Count());
        }
    }
}
