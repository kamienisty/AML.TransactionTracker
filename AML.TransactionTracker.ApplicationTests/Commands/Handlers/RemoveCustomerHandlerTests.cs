using AML.TransactionTracker.Application.Commands.Handlers;
using AML.TransactionTracker.Application.Commands;
using AML.TransactionTracker.Core.Repositories;
using Moq;
using NUnit.Framework;

namespace AML.TransactionTracker.ApplicationTests.Commands.Handlers
{
    public class RemoveCustomerHandlerTests
    {
        private Mock<ICustomerRepository> _repositoryMock = new Mock<ICustomerRepository>();

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
        }

        [Test]
        public async Task Handle_DoesNotCallDeleteMethod_WhenTransactionDoesNotExist()
        {
            var command = new RemoveCustomer(Guid.NewGuid());
            _repositoryMock.Setup(x => x.ExistsAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));

            var handler = new RemoveCustomerHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(
                x => x.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public async Task Handle_DoesNotCallDeleteMethod_WhenTransactionIsMarkedAsDeleted()
        {
            var command = new RemoveCustomer(Guid.NewGuid());
            _repositoryMock.Setup(x => x.ExistsAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            _repositoryMock.Setup(x => x.IsDeletedAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));

            var handler = new RemoveCustomerHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(
                x => x.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public async Task Handle_CallsDeleteMethod_WhenTransactionExistsAndIsNotDeleted()
        {
            var command = new RemoveCustomer(Guid.NewGuid());
            _repositoryMock.Setup(x => x.ExistsAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            _repositoryMock.Setup(x => x.IsDeletedAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));

            var handler = new RemoveCustomerHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(
                x => x.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
