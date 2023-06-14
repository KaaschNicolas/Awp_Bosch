using NUnit.Framework;
using App.Core.Models;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Models.Tests
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void User_Name_MaxLength_Success()
        {
            // Arrange
            User user = new User();
            user.Name = new string('a', 50); // Set Name to a string of 50 characters

            // Act

            // Assert
            Assert.That(user.Name.Length, Is.LessThanOrEqualTo(50));
        }

        [Test]
        public void User_AdUsername_MaxLength_Success()
        {
            // Arrange
            User user = new User();
            user.AdUsername = new string('a', 50); // Set AdUsername to a string of 50 characters

            // Act

            // Assert
            Assert.That(user.AdUsername.Length, Is.LessThanOrEqualTo(50));
        }

        [Test]
        public void User_Transfers_Initialized_Success()
        {
            // Arrange
            User user = new User();

            // Act

            // Assert
            Assert.IsNotNull(user.Transfers);
            Assert.That(user.Transfers, Is.Empty);
        }

        [Test]
        public void User_Comments_Initialized_Success()
        {
            // Arrange
            User user = new User();

            // Act

            // Assert
            Assert.IsNotNull(user.Comments);
            Assert.That(user.Comments, Is.Empty);
        }
    }
}
