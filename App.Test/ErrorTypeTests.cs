using App.Core.Models;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Models.Tests
{
    [TestFixture]
    public class ErrorTypeTests
    {
        [Test]
        public void ErrorType_Properties_Are_Set_Correctly()
        {
            // Arrange
            var errorType = new ErrorType();

            // Act
            errorType.Code = "123";
            errorType.ErrorDescription = "Sample error description";

            // Assert
            Assert.AreEqual("123", errorType.Code);
            Assert.AreEqual("Sample error description", errorType.ErrorDescription);
        }

        [Test]
        public void ErrorType_Pcb_Initialized_Correctly()
        {
            // Arrange
            var errorType = new ErrorType();

            // Act
            errorType.Pcb = new Pcb();

            // Assert
            Assert.IsNotNull(errorType.Pcb);
            Assert.IsInstanceOf<Pcb>(errorType.Pcb);
        }
    }
}
