using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Exceptions;
using NUnit.Framework;

namespace AML.TransactionTracker.Core.Tests.Entities
{
    public class CustomerTests
    {
        [Test]
        public void Constructor_WithFutureBirthdate_ShouldThrowException()
        {
            Assert.Throws<InvalidDateException>(() =>
                new Customer(Guid.NewGuid(), "name", "surname",
                    DateTime.Now.AddYears(18), "address"));
        }

        [Test]
        public void Constructor_WithEmptyGuid_ShouldThrowException()
        {
            Assert.Throws<InvalidEntityIdException>(() =>
                new Customer(Guid.Empty, "name", "surname",
                    DateTime.Now.AddYears(18), "address"));
        }

        [Test]
        public void Constructor_WithoutGuid_ShouldCreateobject()
        {
            var birthdate = DateTime.Now.AddYears(-18);

            var result = new Customer("name", "surname",
                    birthdate, "address", Enumerable.Empty<Transaction>());

            Assert.Multiple(() =>
                {
                    Assert.IsNotNull(result);
                    Assert.IsTrue(result.Id != Guid.Empty);
                    Assert.AreEqual("name", result.Name);
                    Assert.AreEqual("surname", result.Surename);
                    Assert.AreEqual(birthdate, result.Birthdate);
                    Assert.AreEqual("address", result.Address);
                }
            );
        }

        [Test]
        public void Constructor_WithGuid_ShouldCreateobject()
        {
            var birthdate = DateTime.Now.AddYears(-18);
            var id = Guid.NewGuid();

            var result = new Customer(id, "name", "surname",
                    birthdate, "address");

            Assert.Multiple(() =>
                {
                    Assert.IsNotNull(result);
                    Assert.IsTrue(result.Id != Guid.Empty);
                    Assert.AreEqual(id, result.Id);
                    Assert.AreEqual("name", result.Name);
                    Assert.AreEqual("surname", result.Surename);
                    Assert.AreEqual(birthdate, result.Birthdate);
                    Assert.AreEqual("address", result.Address);
                }
            );
        }
    }
}
