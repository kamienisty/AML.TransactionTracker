using AML.TransactionTracker.Application.Queries.Handlers;
using AML.TransactionTracker.Application.Queries;
using AML.TransactionTracker.Core.Repositories;
using Moq;
using NUnit.Framework;

namespace AML.TransactionTracker.ApplicationTests.Queries.Handlers
{
    public class GetFlaggedTransactionsHandlerTests
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
            var command = new GetFlaggedTransactions();
            var handler = new GetFlaggedTransactionsHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(
                x => x.GetFlaggedAsync(), Times.Once);
        }
    }
}
