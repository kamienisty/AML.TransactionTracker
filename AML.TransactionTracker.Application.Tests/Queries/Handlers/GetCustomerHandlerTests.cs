using AML.TransactionTracker.Core.Repositories;
using Moq;
using AML.TransactionTracker.Application.Queries;
using AML.TransactionTracker.Application.Queries.Handlers;
using NUnit.Framework;

namespace AML.TransactionTracker.ApplicationTests.Queries.Handlers
{
    public class GetCustomerHandlerTests
    {
        private Mock<ICustomerRepository> _repositoryMock = new Mock<ICustomerRepository>();

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
        }

        [Test]
        public async Task Handle_CallsGetMethod_WhenGuidIsPassed()
        {
            var command = new GetCustomer(Guid.NewGuid());
            var handler = new GetCustomerHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(
                x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
