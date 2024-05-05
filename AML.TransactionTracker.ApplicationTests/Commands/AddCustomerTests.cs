using AML.TransactionTracker.Application.Commands;
using NUnit.Framework;

namespace AML.TransactionTracker.ApplicationTests.Commands
{
    public class AddCustomerTests
    {
        [Test]
        public void Constructor_CreatesCommand_FromPassedParameters()
        {
            var name = "name";
            var surenamne = "surename";
            var birthdate = DateTime.Parse("1999-01-01");
            var address = "address";

            var result = new AddCustomer(name, surenamne, birthdate, address);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(name, result.Name);
                Assert.AreEqual(surenamne, result.Surename);
                Assert.AreEqual(birthdate, result.Birthdate);
                Assert.AreEqual(address, result.Address);
            });
        }
    }
}
