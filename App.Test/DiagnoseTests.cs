using App.Core.Models;
using NUnit.Framework;
using System.Collections.Generic;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Models.Tests
{
    [TestFixture]
    public class DiagnoseTests
    {
        [Test]
        public void Diagnose_Properties_Are_Set_Correctly()
        {
            // Arrange
            var diagnose = new Diagnose();

            // Act
            diagnose.Name = "Sample Diagnose";

            // Assert
            Assert.AreEqual("Sample Diagnose", diagnose.Name);
        }

        [Test]
        public void Diagnose_Pcbs_Initialized_Correctly()
        {
            // Arrange
            var diagnose = new Diagnose();

            // Act
            diagnose.Pcbs = new List<Pcb>();

            // Assert
            Assert.IsNotNull(diagnose.Pcbs);
            Assert.IsInstanceOf<List<Pcb>>(diagnose.Pcbs);
        }

        [Test]
        public void Diagnose_Equals_Returns_True_For_Same_Object()
        {
            // Arrange
            var diagnose = new Diagnose
            {
                Id = 1,
                Name = "Sample Diagnose"
            };

            // Act
            var result = diagnose.Equals(diagnose);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Diagnose_Equals_Returns_False_For_Different_Object()
        {
            // Arrange
            var diagnose1 = new Diagnose
            {
                Id = 1,
                Name = "Sample Diagnose"
            };

            var diagnose2 = new Diagnose
            {
                Id = 2,
                Name = "Other Diagnose"
            };

            // Act
            var result = diagnose1.Equals(diagnose2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Diagnose_GetHashCode_Returns_Consistent_HashCode()
        {
            // Arrange
            var diagnose = new Diagnose
            {
                Id = 1,
                Name = "Sample Diagnose"
            };

            // Act
            var hashCode1 = diagnose.GetHashCode();
            var hashCode2 = diagnose.GetHashCode();

            // Assert
            Assert.AreEqual(hashCode1, hashCode2);
        }
    }
}
