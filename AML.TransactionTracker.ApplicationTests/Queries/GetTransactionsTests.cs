using AML.TransactionTracker.Application.Commands;
using AML.TransactionTracker.Application.Queries;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AML.TransactionTracker.ApplicationTests.Queries
{
    public class GetTransactionsTests
    {
        [Test]
        public void Constructor_CreatesCommand_FromPassedParameters()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");

            var result = new GetTransaction(id);

            Assert.AreEqual(id, result.Id);
        }
    }
}
