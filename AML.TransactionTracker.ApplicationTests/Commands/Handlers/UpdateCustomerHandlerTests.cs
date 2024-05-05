using AML.TransactionTracker.Application.Commands;
using AML.TransactionTracker.Application.Commands.Handlers;
using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Repositories;
using Moq;
using NUnit.Framework;

namespace AML.TransactionTracker.ApplicationTests.Commands.Handlers
{
    public class UpdateCustomerHandlerTests
    {
        private Mock<ICustomerRepository> _repositoryMock = new Mock<ICustomerRepository>();

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
        }

        [Test]
        public async Task Hande_CallsUpdateMewthod_WhenUpdateCommandIsPassed()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var name = "name";
            var surenamne = "surename";
            var birthdate = DateTime.Parse("1999-01-01");
            var address = "address";

            var command = new UpdateCustomer(id, name, surenamne, birthdate, address);
            var handler = new UpdateCustomerHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(x => 
                x.UpdateAsync(It.Is<Customer>(
                    y => y.Id == id &&
                    y.Name == name &&
                    y.Surename == surenamne &&
                    y.Birthdate == birthdate &&
                    y.Address == address)), 
                Times.Once);
        }
    }
}
