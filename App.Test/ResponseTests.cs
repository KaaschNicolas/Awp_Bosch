using App.Core.Models.Enums;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Models.Tests
{
    [TestFixture]
    public class ResponseTests
    {
        [Test]
        public void Constructor_WithCodeAndData_SetsCodeAndData()
        {
            // Arrange
            var code = ResponseCode.Success;
            var data = "Test data";

            // Act
            var response = new Response<string>(code, data);

            // Assert
            Assert.AreEqual(code, response.Code);
            Assert.AreEqual(data, response.Data);
            Assert.IsNull(response.ErrorMessage);
        }

        [Test]
        public void Constructor_WithCodeAndError_SetsCodeAndErrorMessage()
        {
            // Arrange
            var code = ResponseCode.Error;
            var error = "Test error";

            // Act
            var response = new Response<object>(code, error);

            // Assert
            Assert.AreEqual(code, response.Code);
            Assert.IsNull(response.Data);
            Assert.AreEqual(error, response.ErrorMessage);
        }

        [Test]
        public void Constructor_WithCodeOnly_SetsCode()
        {
            // Arrange
            var code = ResponseCode.None;

            // Act
            var response = new Response<int>(code);

            // Assert
            Assert.AreEqual(code, response.Code);
            Assert.IsNull(response.Data);
            Assert.IsNull(response.ErrorMessage);
        }

        [Test]
        public void DefaultConstructor_SetsDefaultValues()
        {
            // Act
            var response = new Response<decimal>();

            // Assert
            Assert.AreEqual(ResponseCode.Success, response.Code);
            Assert.AreEqual(0m, response.Data);
            Assert.IsNull(response.ErrorMessage);
        }
    }
}
