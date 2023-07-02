using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Models.Tests
{
    [TestFixture]
    public class AuditEntryTests
    {
        [Test]
        public void Message_SetAndGet_Successfully()
        {
            // Arrange
            string message = "Test message";
            AuditEntry auditEntry = new AuditEntry();

            // Act
            auditEntry.Message = message;
            string result = auditEntry.Message;

            // Assert
            Assert.AreEqual(message, result);
        }

        [Test]
        public void User_SetAndGet_Successfully()
        {
            // Arrange
            User user = new User();
            AuditEntry auditEntry = new AuditEntry();

            // Act
            auditEntry.User = user;
            User result = auditEntry.User;

            // Assert
            Assert.AreEqual(user, result);
        }

        [Test]
        public void Level_SetAndGet_Successfully()
        {
            // Arrange
            string level = "Info";
            AuditEntry auditEntry = new AuditEntry();

            // Act
            auditEntry.Level = level;
            string result = auditEntry.Level;

            // Assert
            Assert.AreEqual(level, result);
        }

        [Test]
        public void Exception_SetAndGet_Successfully()
        {
            // Arrange
            string exception = "Test exception";
            AuditEntry auditEntry = new AuditEntry();

            // Act
            auditEntry.Exception = exception;
            string result = auditEntry.Exception;

            // Assert
            Assert.AreEqual(exception, result);
        }
    }
}
