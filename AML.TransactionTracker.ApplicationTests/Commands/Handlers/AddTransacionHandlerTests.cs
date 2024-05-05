using AML.TransactionTracker.Application.Commands.Handlers;
using AML.TransactionTracker.Application.Commands;
using AML.TransactionTracker.Application.Exceptions;
using AML.TransactionTracker.Core.Repositories;
using Moq;
using static AML.TransactionTracker.Core.Enums.EnumsCore;
using AML.TransactionTracker.Core.Entities;
using NUnit.Framework;
using AML.TransactionTracker.Application.Services;
using AML.TransactionTracker.Core.Events;

namespace AML.TransactionTracker.ApplicationTests.Commands.Handlers
{
    public class AddTransacionHandlerTests
    {
        private Mock<ITransactionRepository> _repositoryMock = new Mock<ITransactionRepository>();

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ITransactionRepository>();
        }

        [Test]
        public void Handle_ThrowsException_WhenCustomerAlreadyExists()
        {
            var command = new AddTransaction(100, Currency.USD, TransactionType.Deposit,
                Guid.NewGuid(), DateTime.Now, "description");
            _repositoryMock.Setup(x => x.ExistsAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var eventPublisherMock = new Mock<IEventPublisher>();

            var handler = new AddTransactionHandler(_repositoryMock.Object, eventPublisherMock.Object);

            Assert.ThrowsAsync<EntityAlreadyExistsException>(async () => await handler.Handle(command, new CancellationToken()));
        }

        [Test]
        public async Task Handle_SavesNewEntity_WhenCalled()
        {
            var amount = 100;
            var currency = Currency.USD;
            var type = TransactionType.Deposit;
            var customerId = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var date = DateTime.Parse("1999-01-01");
            var description = "description";

            var command = new AddTransaction(amount, currency, type, customerId, date, description);
            _repositoryMock.Setup(x => x.ExistsAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            var eventPublisherMock = new Mock<IEventPublisher>();

            var handler = new AddTransactionHandler(_repositoryMock.Object, eventPublisherMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(
                x => x.AddAsync(It.Is<Transaction>(
                    y => y.Amount == amount &&
                    y.TransactionCurrency == currency &&
                    y.Type == type &&
                    y.CustomerId == customerId &&
                    y.Date == date &&
                    y.Description == description))
            , Times.Once);
            eventPublisherMock.Verify(x => x.PublishAsync(It.IsAny<IEvent>()), Times.Once);
        }
    }
}
