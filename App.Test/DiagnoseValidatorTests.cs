using App.Core.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Validator.Tests
{
    [TestFixture]
    public class DiagnoseValidatorTests
    {
        [Test]
        public void Name_Required_Successfully()
        {
            // Arrange
            Diagnose diagnose = new Diagnose();
            DiagnoseValidator diagnoseValidator = new DiagnoseValidator(diagnose);

            // Act
            bool isValid = diagnoseValidator.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void Name_MaxLength_Successfully()
        {
            // Arrange
            Diagnose diagnose = new Diagnose();
            diagnose.Name = new string('a', 650); // Set Name to a string of 650 characters
            DiagnoseValidator diagnoseValidator = new DiagnoseValidator(diagnose);

            // Act
            bool isValid = diagnoseValidator.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void GetErrors_ReturnsValidationErrors_Successfully()
        {
            // Arrange
            Diagnose diagnose = new Diagnose();
            diagnose.Name = null; // Set Name to null
            DiagnoseValidator diagnoseValidator = new DiagnoseValidator(diagnose);

            // Act
            List<string> errors = diagnoseValidator.GetErrors();

            // Assert
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Name darf nicht null sein oder 650 Zeichen überschreiten.", errors[0]);
        }
    }
}
