using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Exceptions;
using NUnit.Framework;
using static AML.TransactionTracker.Core.Enums.EnumsCore;

namespace AML.TransactionTracker.Core.Tests.Entities
{
    public class TransactionTests
    {
        [Test]
        public void Constructor_WithEmptyGuid_ShouldThrowException()
        {
            Assert.Throws<InvalidEntityIdException>(() => new Transaction(Guid.Empty, 100, Currency.USD, TransactionType.Deposit,
                Guid.NewGuid(), DateTime.Now, "description"));
        }

        [Test]
        public void Constructor_WithFutureTransactionDate_ShouldThrowException()
        {
            Assert.Throws<InvalidDateException>(() => new Transaction(Guid.NewGuid(), 100, Currency.USD, TransactionType.Deposit,
                Guid.NewGuid(), DateTime.Now.AddDays(5), "description"));
        }

        [Test]
        public void Constructor_WithoutGuid_ShouldCreateObject()
        {
            var customerId = Guid.NewGuid();
            var date = DateTime.Now.AddDays(-6);

            var result = new Transaction(100, Currency.USD, TransactionType.Deposit, customerId, date, "description");

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.Id != Guid.Empty);
                Assert.AreEqual(100, result.Amount);
                Assert.AreEqual(Currency.USD, result.TransactionCurrency);
                Assert.AreEqual(TransactionType.Deposit, result.Type);
                Assert.AreEqual(customerId, result.CustomerId);
                Assert.AreEqual(date, result.Date);
                Assert.AreEqual("description", result.Description);
            });
            
        }

        [Test]
        public void Constructor_WithGuid_ShouldCreateObject()
        {
            var id = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var date = DateTime.Now.AddDays(-6);

            var result = new Transaction(id, 100, Currency.USD, TransactionType.Deposit, customerId, date, "description");

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.Id != Guid.Empty);
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(100, result.Amount);
                Assert.AreEqual(Currency.USD, result.TransactionCurrency);
                Assert.AreEqual(TransactionType.Deposit, result.Type);
                Assert.AreEqual(customerId, result.CustomerId);
                Assert.AreEqual(date, result.Date);
                Assert.AreEqual("description", result.Description);
            });

        }
    }
}
