using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Infrastructure.SQLite;
using AML.TransactionTracker.Infrastructure.SQLite.Repository;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using RulesEngine.Models;
using static AML.TransactionTracker.Core.Enums.EnumsCore;

namespace AML.TransactionTracker.Infrastructure.Tests.Repositories
{
    public class TransactionSQLiteRepositoryTest
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
            var mockSet = new Mock<DbSet<Transaction>>();
            _context.Setup(x => x.Transactions).Returns(mockSet.Object);
            var customerId = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");

            var transaction = new Transaction(100, Currency.USD, TransactionType.Deposit, customerId, DateTime.Now, "description");
            var repo = new TransactionSQLiteRepository(_context.Object);

            await repo.AddAsync(transaction);

            mockSet.Verify(x => x.AddAsync(It.IsAny<Transaction>(), new CancellationToken()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetAsync_ReturnsTransaction_WhenCorrectIdIsUsed()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var customerId = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111");
            var data = new List<Transaction>
            {
                new Transaction(id, 100, Currency.PLN, TransactionType.Deposit, customerId, DateTime.Now, "description")
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockSet.Object);
            var repo = new TransactionSQLiteRepository(_context.Object);

            var result = await repo.GetAsync(id);

            Assert.NotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [Test]
        public async Task GetAsync_ReturnsNull_WhenTransactionDoesNotExists()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var customerId = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111");
            var data = new List<Transaction>
            {
                new Transaction(Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0222"), 100, Currency.PLN, TransactionType.Deposit, customerId, DateTime.Now, "description")
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockSet.Object);
            var repo = new TransactionSQLiteRepository(_context.Object);

            var result = await repo.GetAsync(id);

            Assert.IsNull(result);
        }

        [Test]
        public async Task DeleteAsync_RemovesTransaction_WhenCorrectIdIsUsed()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var customerId = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111");
            var data = new List<Transaction>
            {
                new Transaction(id, 100, Currency.PLN, TransactionType.Deposit, customerId, DateTime.Now, "description")
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockSet.Object);
            var repo = new TransactionSQLiteRepository(_context.Object);

            await repo.DeleteAsync(id);

            mockSet.Verify(x => x.Remove(It.Is<Transaction>(y => y.Id == id)), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_DoesNothing_WhenTransactionWithGivenIdIsNoInDatabase()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var customerId = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111");
            var data = new List<Transaction>
            {
                new Transaction(Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0222"), 
                    100, Currency.PLN, TransactionType.Deposit, customerId, DateTime.Now, "description")
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockSet.Object);
            var repo = new TransactionSQLiteRepository(_context.Object);

            await repo.DeleteAsync(id);

            mockSet.Verify(x => x.Remove(It.Is<Transaction>(y => y.Id == id)), Times.Never);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task ExistAsync_ReturnsTrue_WhenItemExists()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var customerId = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111");
            var data = new List<Transaction>
            {
                new Transaction(id, 100, Currency.PLN, TransactionType.Deposit, 
                    customerId, DateTime.Now, "description")
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockSet.Object);
            var repo = new TransactionSQLiteRepository(_context.Object);

            var result = await repo.ExistsAsync(id);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task ExistAsync_ReturnsFalse_WhenItemDoesNotExists()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var customerId = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111");
            var data = new List<Transaction>
            {
                new Transaction(Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111"), 100, Currency.PLN, 
                    TransactionType.Deposit, customerId, DateTime.Now, "description")
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockSet.Object);
            var repo = new TransactionSQLiteRepository(_context.Object);

            var result = await repo.ExistsAsync(id);

            Assert.IsFalse(result);
        }

        [Test]
        public void GetAll_ReturnsQueryableCollection_WhenCalled()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var customerId = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111");
            var data = new List<Transaction>
            {
                new Transaction(Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111"), 100, Currency.PLN,
                    TransactionType.Deposit, customerId, DateTime.Now, "description")
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockSet.Object);
            var repo = new TransactionSQLiteRepository(_context.Object);

            var result = repo.GetAll();

            Assert.IsTrue(result is IQueryable<Transaction>);
            Assert.IsTrue(result.Any());
        }

        [Test]
        public async Task GetFlaggedAsync_ReturnsFlaggedTransactions_WhenCalled()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var customerId = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111");
            var data = new List<Transaction>
            {
                new Transaction(id, 100, Currency.PLN, TransactionType.Deposit, customerId, DateTime.Now, 
                "description", true)
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockSet.Object);
            var repo = new TransactionSQLiteRepository(_context.Object);

            var result = await repo.GetFlaggedAsync();

            Assert.NotNull(result);
            Assert.IsTrue(result.Count() == 1);
        }

        [Test]
        public async Task GetFlaggedAsync_ReturnsEmptyCollection_WhenThereAreNoFlaggedTransactions()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var customerId = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111");
            var data = new List<Transaction>
            {
                new Transaction(id, 100, Currency.PLN, TransactionType.Deposit, customerId, DateTime.Now, "description")
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockSet.Object);
            var repo = new TransactionSQLiteRepository(_context.Object);

            var result = await repo.GetFlaggedAsync();

            Assert.NotNull(result);
            Assert.IsFalse(result.Any());
        }

        [Test]
        public async Task FlagTransactionAsync_SetsFlaggedToTrue_WhenItWasFlasePreviously()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var customerId = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111");
            var data = new List<Transaction>
            {
                new Transaction(id, 100, Currency.PLN, TransactionType.Deposit, customerId, DateTime.Now,
                "description", true)
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockSet.Object);
            var repo = new TransactionSQLiteRepository(_context.Object);

            await repo.FlagTransactionAsync(id);

            Assert.IsTrue(data.First().Flagged);
        }

        [TestCase("2000-01-01", 0)]
        [TestCase("1999-10-05", 1)]
        [TestCase("1999-10-01", 2)]
        public async Task GetNumberOfTransactionsSinceDate_ReturnsCorrectNumber_ForPassedDate(DateTime date, int resultNo)
        {
            var customerId = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0111");
            var data = new List<Transaction>
            {
                new Transaction(Guid.NewGuid(), 100, Currency.PLN, TransactionType.Deposit, customerId, DateTime.Parse("1999-10-02"), "description"),
                new Transaction(Guid.NewGuid(), 100, Currency.PLN, TransactionType.Deposit, customerId, DateTime.Parse("1999-10-07"), "description")
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockSet.Object);
            var repo = new TransactionSQLiteRepository(_context.Object);

            var result = await repo.GetNumberOfTransactionsSinceDate(date, customerId) ;

            Assert.AreEqual(resultNo, result);
        }

        [Test]
        public async Task GetRulesForTransactionAsync_ReturnsCorrectWorkflow_ForGivenName()
        {
            var data = new List<Workflow>
            {
                new Workflow{ WorkflowName = "workflow", Rules = new List<Rule>()}
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Workflows).Returns(mockSet.Object);
            var repo = new TransactionSQLiteRepository(_context.Object);

            var result = await repo.GetRulesForTransactionAsync("workflow");

            Assert.IsTrue(result.Count() == 1);
            Assert.AreEqual("workflow", result.First().WorkflowName);
        }

        [Test]
        public async Task GetRulesForTransactionAsync_ReturnsEmptyCollection_WhenThereIsNoWorkflowWithMatchingName()
        {
            var data = new List<Workflow>
            {
                new Workflow{ WorkflowName = "workflow", Rules = new List<Rule>()}
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Workflows).Returns(mockSet.Object);
            var repo = new TransactionSQLiteRepository(_context.Object);

            var result = await repo.GetRulesForTransactionAsync("workflow1");

            Assert.IsTrue(result.Count() == 0);
        }
    }
}
