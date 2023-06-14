using App.Core.Models;
using NUnit.Framework;
using System.Collections.Generic;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Validator.Tests
{
    [TestFixture]
    public class TransferValidatorTests
    {
        [Test]
        public void StorageLocation_Required_Successfully()
        {
            // Arrange
            Transfer transfer = new Transfer();
            TransferValidator transferValidator = new TransferValidator(transfer);

            // Act
            bool isValid = transferValidator.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void NotedBy_Required_Successfully()
        {
            // Arrange
            Transfer transfer = new Transfer();
            TransferValidator transferValidator = new TransferValidator(transfer);

            // Act
            bool isValid = transferValidator.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void GetErrors_ReturnsValidationErrors_Successfully()
        {
            // Arrange
            Transfer transfer = new Transfer();
            TransferValidator transferValidator = new TransferValidator(transfer);

            // Act
            List<string> errors = transferValidator.GetErrors();

            // Assert
            Assert.AreEqual(2, errors.Count);
            Assert.Contains("StorageLocation darf nicht null sein.", errors);
            Assert.Contains("NotedBy darf nicht null sein.", errors);
        }
    }
}
