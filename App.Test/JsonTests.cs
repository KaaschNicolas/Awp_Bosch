using Newtonsoft.Json;
using NUnit.Framework;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Helpers.Tests
{
    [TestFixture]
    public class JsonTests
    {
        [Test]
        public async Task ToObjectAsync_WithValidJson_ReturnsDeserializedObject()
        {
            // Arrange
            string json = "{\"name\":\"John\",\"age\":30,\"city\":\"New York\"}";

            // Act
            var result = await Json.ToObjectAsync<Person>(json);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("John", result.Name);
            Assert.AreEqual(30, result.Age);
            Assert.AreEqual("New York", result.City);
        }

        [Test]
        public async Task StringifyAsync_WithValidObject_ReturnsSerializedJson()
        {
            // Arrange
            var person = new Person { Name = "John", Age = 30, City = "New York" };

            // Act
            var result = await Json.StringifyAsync(person);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("{\"Name\":\"John\",\"Age\":30,\"City\":\"New York\"}", result);
        }

        // Add more test cases for edge cases, invalid JSON, etc. as needed

        private class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string City { get; set; }
        }
    }
}
