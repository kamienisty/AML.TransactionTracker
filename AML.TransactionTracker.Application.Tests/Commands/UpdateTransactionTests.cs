using AML.TransactionTracker.Application.Commands;
using NUnit.Framework;
using static AML.TransactionTracker.Core.Enums.EnumsCore;

namespace AML.TransactionTracker.ApplicationTests.Commands
{
    public class updateTransactionTests
    {
        [Test]
        public void Constructor_CreatesCommand_FromPassedParameters()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var amount = 100;
            var currency = Currency.EURO;
            var type = TransactionType.Deposit;
            var customerId = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var date = DateTime.Parse("1999-01-01");
            var description = "description";

            var result = new UpdateTransaction(id, amount, currency, type, customerId, date, description);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(amount, result.Amount);
                Assert.AreEqual(currency, result.TransactionCurrency);
                Assert.AreEqual(type, result.Type);
                Assert.AreEqual(customerId, result.CustomerId);
                Assert.AreEqual(date, result.Date);
                Assert.AreEqual(description, result.Description);
            });
        }
    }
}
