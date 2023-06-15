using App.Core.Helpers;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Helpers.Tests
{
    [TestFixture]
    public class PropertyHelperTests
    {
        [Test]
        public void PropertiesToString_Returns_Correct_String()
        {
            // Arrange
            var obj = new { Name = "John", Age = 30, City = "New York" };

            // Act
            var result = obj.PropertiesToString();

            // Assert
            Assert.AreEqual("{Name:John,Age:30,City:New York}", result);
        }
    }
}
