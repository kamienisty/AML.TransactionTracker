using AML.TransactionTracker.Application.Commands;
using AML.TransactionTracker.Application.Commands.Handlers;
using AML.TransactionTracker.Core.Repositories;
using Moq;
using NUnit.Framework;

namespace AML.TransactionTracker.ApplicationTests.Commands.Handlers
{
    public  class FlagTransactionHandlerTests
    {
        private Mock<ITransactionRepository> _repositoryMock = new Mock<ITransactionRepository>();

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ITransactionRepository>();
        }

        [Test]
        public async Task Handle_CallsFlagMethod_WhenGuidIsPassed()
        {
            var command = new FlagTransaction(Guid.NewGuid());

            var handler = new FlagTransactionHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(
                x => x.FlagTransactionAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
