using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using App.Core.Models.Enums;

namespace App.Core.Models.Tests
{
    [TestFixture]
    public class ResponseCodeTests
    {
        [Test]
        public void ResponseCode_Success_ValueCorrect()
        {
            // Arrange
            ResponseCode responseCode = ResponseCode.Success;

            // Act

            // Assert
            Assert.AreEqual("Success", responseCode.ToString());
        }

        [Test]
        public void ResponseCode_Error_ValueCorrect()
        {
            // Arrange
            ResponseCode responseCode = ResponseCode.Error;

            // Act

            // Assert
            Assert.AreEqual("Error", responseCode.ToString());
        }

        [Test]
        public void ResponseCode_None_ValueCorrect()
        {
            // Arrange
            ResponseCode responseCode = ResponseCode.None;

            // Act

            // Assert
            Assert.AreEqual("None", responseCode.ToString());
        }
    }
}
