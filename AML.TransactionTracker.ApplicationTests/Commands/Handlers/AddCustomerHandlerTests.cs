using AML.TransactionTracker.Application.Commands;
using AML.TransactionTracker.Application.Commands.Handlers;
using AML.TransactionTracker.Application.Exceptions;
using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Repositories;
using Moq;
using NUnit.Framework;

namespace AML.TransactionTracker.ApplicationTests.Commands.Handlers
{
    public class AddCustomerHandlerTests
    {
        private Mock<ICustomerRepository> _repositoryMock = new Mock<ICustomerRepository>();

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
        }

        [Test]
        public void Handle_ThrowsException_WhenCustomerAlreadyExists()
        {
            var command = new AddCustomer("name", "surename", DateTime.Now.AddDays(-1), "address");
            _repositoryMock.Setup(x => x.ExistsAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));

            var handler = new AddCustomerHandler(_repositoryMock.Object);

            Assert.ThrowsAsync<EntityAlreadyExistsException>(async () => await handler.Handle(command, new CancellationToken()));
        }

        [Test]
        public async Task Handle_SavesNewEntity_WhenCalled()
        {
            var name = "name";
            var surenamne = "surename";
            var birthdate = DateTime.Parse("1999-01-01");
            var address = "address";

            var command = new AddCustomer(name, surenamne, birthdate, address);
            _repositoryMock.Setup(x => x.ExistsAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));

            var handler = new AddCustomerHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(x => x.AddAsync(
                It.Is<Customer>(
                    y => y.Name == "name" &&
                    y.Surename == "surename" &&
                    y.Birthdate == birthdate &&
                    y.Address == address)),
                Times.Once
            );
        }
    }
}
