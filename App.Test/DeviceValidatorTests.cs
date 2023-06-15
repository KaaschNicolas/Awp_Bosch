using App.Core.Models;
using NUnit.Framework;
using System.Collections.Generic;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Validator.Tests
{
    [TestFixture]
    public class DeviceValidatorTests
    {
        [Test]
        public void Name_Required_Successfully()
        {
            // Arrange
            Device device = new Device();
            DeviceValidator deviceValidator = new DeviceValidator(device);

            // Act
            bool isValid = deviceValidator.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void Name_MaxLength_Successfully()
        {
            // Arrange
            Device device = new Device();
            device.Name = new string('a', 50); // Set Name to a string of 50 characters
            DeviceValidator deviceValidator = new DeviceValidator(device);

            // Act
            bool isValid = deviceValidator.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void GetErrors_ReturnsValidationErrors_Successfully()
        {
            // Arrange
            Device device = new Device();
            device.Name = null; // Set Name to null
            DeviceValidator deviceValidator = new DeviceValidator(device);

            // Act
            List<string> errors = deviceValidator.GetErrors();

            // Assert
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Name darf nicht null sein oder 50 Zeichen überschreiten.", errors[0]);
        }
    }
}
