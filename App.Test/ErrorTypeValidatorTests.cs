using App.Core.Models;
using NUnit.Framework;
using System.Collections.Generic;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Validator.Tests
{
    [TestFixture]
    public class ErrorTypeValidatorTests
    {
        [Test]
        public void Code_Required_Successfully()
        {
            // Arrange
            ErrorType errorType = new ErrorType();
            ErrorTypeValidator errorTypeValidator = new ErrorTypeValidator(errorType);

            // Act
            bool isValid = errorTypeValidator.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void Code_MaxLength_Successfully()
        {
            // Arrange
            ErrorType errorType = new ErrorType();
            errorType.Code = new string('a', 5); // Set Code to a string of 5 characters
            ErrorTypeValidator errorTypeValidator = new ErrorTypeValidator(errorType);

            // Act
            bool isValid = errorTypeValidator.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void ErrorDescription_Required_Successfully()
        {
            // Arrange
            ErrorType errorType = new ErrorType();
            ErrorTypeValidator errorTypeValidator = new ErrorTypeValidator(errorType);

            // Act
            bool isValid = errorTypeValidator.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void ErrorDescription_MaxLength_Successfully()
        {
            // Arrange
            ErrorType errorType = new ErrorType();
            errorType.ErrorDescription = new string('a', 650); // Set ErrorDescription to a string of 650 characters
            ErrorTypeValidator errorTypeValidator = new ErrorTypeValidator(errorType);

            // Act
            bool isValid = errorTypeValidator.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void GetErrors_ReturnsValidationErrors_Successfully()
        {
            // Arrange
            ErrorType errorType = new ErrorType();
            errorType.Code = null; // Set Code to null
            ErrorTypeValidator errorTypeValidator = new ErrorTypeValidator(errorType);

            // Act
            List<string> errors = errorTypeValidator.GetErrors();

            // Assert
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Code darf nicht null sein oder 5 Zeichen überschreiten.", errors[0]);
        }
    }
}
