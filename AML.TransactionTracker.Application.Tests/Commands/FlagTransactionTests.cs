using AML.TransactionTracker.Application.Commands;
using NUnit.Framework;

namespace AML.TransactionTracker.ApplicationTests.Commands
{
    public class FlagTransactionTests
    {
        [Test]
        public void Constructor_CreatesCommand_FromPassedParameters()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");

            var result = new FlagTransaction(id);

            Assert.AreEqual(id, result.Id);
        }
    }
}
