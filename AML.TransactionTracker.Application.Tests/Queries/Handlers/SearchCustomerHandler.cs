using AML.TransactionTracker.Application.Exceptions;
using AML.TransactionTracker.Application.Queries;
using AML.TransactionTracker.Application.Queries.Handlers;
using AML.TransactionTracker.Core.Model;
using AML.TransactionTracker.Core.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AML.TransactionTracker.ApplicationTests.Queries.Handlers
{
    public class SearchCustomerHandlerTests
    {
        private Mock<ICustomerRepository> _repositoryMock = new Mock<ICustomerRepository>();

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
        }

        [Test]
        public void SearchCustomerHandler_ThrowsException_WhenAllParametersAreEmpty()
        {
            var command = new SearchCustomer(string.Empty, string.Empty, null, string.Empty);
            var handler = new SearchCustomerHandler(_repositoryMock.Object);

            Assert.ThrowsAsync<EmptyParametersException>(async () => await handler.Handle(command, new CancellationToken()));
        }

        [TestCase("name", null, null, null)]
        [TestCase(null, "surename", null, null)]
        [TestCase(null, null, "1999-01-01", null)]
        [TestCase(null, null, null, "address")]
        public async Task SearchCustomerHandler_CallsSearchMethod_WhenAtLeastOneparameterInSotEmpty(string name, string surename, DateTime? date, string address)
        {
            var command = new SearchCustomer(name, surename, date, address);
            var handler = new SearchCustomerHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(x => x.Search(It.IsAny<SearchModel>()), Times.Once);
        }
    }
}
