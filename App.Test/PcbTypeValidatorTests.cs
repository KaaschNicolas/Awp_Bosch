using App.Core.Models;
using NUnit.Framework;
using System.Collections.Generic;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Validator.Tests
{
    [TestFixture]
    public class PcbTypeValidatorTests
    {
        [Test]
        public void PcbPartNumber_Required_Successfully()
        {
            // Arrange
            PcbType pcbType = new PcbType();
            PcbTypeValidator pcbTypeValidator = new PcbTypeValidator(pcbType);

            // Act
            bool isValid = pcbTypeValidator.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void PcbPartNumber_MaxLength_Successfully()
        {
            // Arrange
            PcbType pcbType = new PcbType();
            pcbType.PcbPartNumber = new string('a', 10); // Set PcbPartNumber to a string of 10 characters
            PcbTypeValidator pcbTypeValidator = new PcbTypeValidator(pcbType);

            // Act
            bool isValid = pcbTypeValidator.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Description_Required_Successfully()
        {
            // Arrange
            PcbType pcbType = new PcbType();
            PcbTypeValidator pcbTypeValidator = new PcbTypeValidator(pcbType);

            // Act
            bool isValid = pcbTypeValidator.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void MaxTransfer_ValidValue_Successfully()
        {
            // Arrange
            PcbType pcbType = new PcbType();
            pcbType.MaxTransfer = 10; // Set MaxTransfer to a valid value
            PcbTypeValidator pcbTypeValidator = new PcbTypeValidator(pcbType);

            // Act
            bool isValid = pcbTypeValidator.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void MaxTransfer_InvalidValue_ReturnsValidationErrors()
        {
            // Arrange
            PcbType pcbType = new PcbType();
            pcbType.MaxTransfer = 0; // Set MaxTransfer to an invalid value
            PcbTypeValidator pcbTypeValidator = new PcbTypeValidator(pcbType);

            // Act
            List<string> errors = pcbTypeValidator.GetErrors();

            // Assert
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("MaxTransfer muss mindestens 1 betragen.", errors[0]);
        }
    }
}
