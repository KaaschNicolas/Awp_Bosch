using App.Core.Models;
using NUnit.Framework;
using System.Collections.Generic;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Validator.Tests
{
    [TestFixture]
    public class AuditEntryValidatorTests
    {
        [Test]
        public void Message_Required_Successfully()
        {
            // Arrange
            AuditEntry auditEntry = new AuditEntry();
            AuditEntryValidator auditEntryValidator = new AuditEntryValidator(auditEntry);

            // Act
            bool isValid = auditEntryValidator.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void Message_MaxLength_Successfully()
        {
            // Arrange
            AuditEntry auditEntry = new AuditEntry();
            auditEntry.Message = new string('a', 250); // Set Message to a string of 250 characters
            AuditEntryValidator auditEntryValidator = new AuditEntryValidator(auditEntry);

            // Act
            bool isValid = auditEntryValidator.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void User_Required_Successfully()
        {
            // Arrange
            AuditEntry auditEntry = new AuditEntry();
            AuditEntryValidator auditEntryValidator = new AuditEntryValidator(auditEntry);

            // Act
            bool isValid = auditEntryValidator.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void Level_MaxLength_Successfully()
        {
            // Arrange
            AuditEntry auditEntry = new AuditEntry();
            auditEntry.Level = new string('a', 20); // Set Level to a string of 20 characters
            AuditEntryValidator auditEntryValidator = new AuditEntryValidator(auditEntry);

            // Act
            bool isValid = auditEntryValidator.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void GetErrors_ReturnsValidationErrors_Successfully()
        {
            // Arrange
            AuditEntry auditEntry = new AuditEntry();
            auditEntry.Message = null; // Set Message to null
            AuditEntryValidator auditEntryValidator = new AuditEntryValidator(auditEntry);

            // Act
            List<string> errors = auditEntryValidator.GetErrors();

            // Assert
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Nachricht darf nicht leer sein oder 250 Zeichen überschreiten.", errors[0]);
        }
    }
}
