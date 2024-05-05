using AML.TransactionTracker.Application.Commands;
using NUnit.Framework;

namespace AML.TransactionTracker.ApplicationTests.Commands
{
    public class UpdateCustomerTests
    {
        [Test]
        public void Constructor_CreatesCommand_FromPassedParameters()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var name = "name";
            var surenamne = "surename";
            var birthdate = DateTime.Parse("1999-01-01");
            var address = "address";

            var result = new UpdateCustomer(id, name, surenamne, birthdate, address);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(name, result.Name);
                Assert.AreEqual(surenamne, result.Surename);
                Assert.AreEqual(birthdate, result.Birthdate);
                Assert.AreEqual(address, result.Address);
            });
        }
    }
}
