using AML.TransactionTracker.Application.Queries;
using NUnit.Framework;

namespace AML.TransactionTracker.ApplicationTests.Queries
{
    public class SearchCustomerTests
    {
        [Test]
        public void Constructor_CreatesCommand_FromPassedParameters()
        {
            var name = "name";
            var surename = "surename";
            var birthdate = DateTime.Parse("1999-05-13");
            var address = "address";

            var result = new SearchCustomer(name, surename, birthdate, address);

            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(surename, result.Surename);
            Assert.AreEqual(birthdate, result.Birthdate);
            Assert.AreEqual(address, result.Address);
        }
    }
}
