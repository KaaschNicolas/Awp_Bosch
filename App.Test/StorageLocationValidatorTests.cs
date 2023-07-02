using App.Core.Models;
using NUnit.Framework;
using System.Collections.Generic;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Validator.Tests
{
    [TestFixture]
    public class StorageLocationValidatorTests
    {
        [Test]
        public void StorageName_Required_Successfully()
        {
            // Arrange
            StorageLocation storageLocation = new StorageLocation();
            StorageLocationValidator storageLocationValidator = new StorageLocationValidator(storageLocation);

            // Act
            bool isValid = storageLocationValidator.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void StorageName_MaxLength_Successfully()
        {
            // Arrange
            StorageLocation storageLocation = new StorageLocation();
            storageLocation.StorageName = new string('a', 50); // Set StorageName to a string of 50 characters
            StorageLocationValidator storageLocationValidator = new StorageLocationValidator(storageLocation);

            // Act
            bool isValid = storageLocationValidator.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void GetErrors_ReturnsValidationErrors_Successfully()
        {
            // Arrange
            StorageLocation storageLocation = new StorageLocation();
            storageLocation.StorageName = null; // Set StorageName to null
            StorageLocationValidator storageLocationValidator = new StorageLocationValidator(storageLocation);

            // Act
            List<string> errors = storageLocationValidator.GetErrors();

            // Assert
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Code darf nicht null sein oder 50 Zeichen überschreiten.", errors[0]);
        }
    }
}
