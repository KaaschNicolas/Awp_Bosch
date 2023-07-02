using App.Core.Helpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Helpers.Tests
{
    [TestFixture]
    public class OrderByHelperTests
    {
        private class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        [Test]
        public void OrderBy_Returns_OrderedQueryable()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person { Name = "John", Age = 30 },
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Bob", Age = 40 }
            }.AsQueryable();

            // Act
            var result = people.OrderBy("Name", desc: false).ToList();

            // Assert
            Assert.AreEqual("Alice", result[0].Name);
            Assert.AreEqual("Bob", result[1].Name);
            Assert.AreEqual("John", result[2].Name);
        }


        private class PaginatedPcb
        {
            public int Id { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime DeletedDate { get; set; }
            public string SerialNumber { get; set; }

        public static List<PaginatedPcb> GetTestData()
        {
            return new List<PaginatedPcb>
                {
                    new PaginatedPcb { Id = 1, CreatedDate = DateTime.Now.AddDays(-2), DeletedDate = DateTime.Now.AddDays(-1), SerialNumber = "P001" },
                    new PaginatedPcb { Id = 2, CreatedDate = DateTime.Now.AddDays(-3), DeletedDate = DateTime.Now.AddDays(-2), SerialNumber = "P002" },
                    new PaginatedPcb { Id = 3, CreatedDate = DateTime.Now.AddDays(-1), DeletedDate = DateTime.Now, SerialNumber = "P003" }
                };
        }
    }

    [Test]
    public void OrderBy_Returns_PCB_simplified_OrderedQueryable()
    {
        // Arrange
        var entities = PaginatedPcb.GetTestData().AsQueryable();

        // Act
        var result = entities.OrderBy("CreatedDate", desc: true).ToList();

        // Assert
        Assert.AreEqual(3, result[0].Id);
        Assert.AreEqual(1, result[1].Id);
        Assert.AreEqual(2, result[2].Id);
    }
    }
}
