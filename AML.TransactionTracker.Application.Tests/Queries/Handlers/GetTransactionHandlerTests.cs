using AML.TransactionTracker.Core.Repositories;
using Moq;
using AML.TransactionTracker.Application.Queries;
using AML.TransactionTracker.Application.Queries.Handlers;
using NUnit.Framework;

namespace AML.TransactionTracker.ApplicationTests.Queries.Handlers
{
    public class GetTransactionHandlerTests
    {
        private Mock<ITransactionRepository> _repositoryMock = new Mock<ITransactionRepository>();

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ITransactionRepository>();
        }

        [Test]
        public async Task Handle_CallsGetMethod_WhenGuidIsPassed()
        {
            var command = new GetTransaction(Guid.NewGuid());
            var handler = new GetTransactionHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(
                x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
