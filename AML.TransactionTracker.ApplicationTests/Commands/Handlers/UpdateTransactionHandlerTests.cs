using AML.TransactionTracker.Application.Commands;
using AML.TransactionTracker.Application.Commands.Handlers;
using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Repositories;
using Moq;
using NUnit.Framework;
using static AML.TransactionTracker.Core.Enums.EnumsCore;

namespace AML.TransactionTracker.ApplicationTests.Commands.Handlers
{
    public class UpdateTransactionHandlerTests
    {
        private Mock<ITransactionRepository> _repositoryMock = new Mock<ITransactionRepository>();

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ITransactionRepository>();
        }

        [Test]
        public async Task Hande_CallsUpdateMewthod_WhenUpdateCommandIsPassed()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da3");
            var amount = 100;
            var currency = Currency.USD;
            var type = TransactionType.Deposit;
            var customerId = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var date = DateTime.Parse("1999-01-01");
            var description = "description";

            var command = new UpdateTransaction(id, amount, currency, type, customerId, date, description);
            var handler = new UpdateTransactionHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(
                x => x.UpdateAsync(It.Is<Transaction>(
                    y => y.Id == id &&
                    y.Amount == amount &&
                    y.TransactionCurrency == currency &&
                    y.Type == type &&
                    y.CustomerId == customerId &&
                    y.Date == date &&
                    y.Description == description)), 
                Times.Once);
        }
    }
}
