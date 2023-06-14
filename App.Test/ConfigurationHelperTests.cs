using App.Core.Helpers;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.IO;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Tests.Helpers
{
    [TestFixture]
    public class ConfigurationHelperTests
    {
        private IConfigurationRoot _configuration;

        [SetUp]
        public void Setup()
        {
            _configuration = ConfigurationHelper.Configuration;
        }

        [Test]
        public void Configuration_Should_Not_Be_Null()
        {
            // Assert
            Assert.IsNotNull(_configuration);
        }

        [Test]
        public void Configuration_Should_Contain_Expected_Values()
        {
            // Arrange
            string expectedValue = "SomeValue";

            // Act
            string actualValue = _configuration["SomeKey"];

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void GetConfigurationBuilder_Should_Return_Builder_Instance()
        {
            // Act
            IConfigurationBuilder builder = ConfigurationHelper.GetConfigurationBuilder();

            // Assert
            Assert.IsNotNull(builder);
        }

        [Test]
        public void CreateConfiguration_Should_Return_Configuration_Instance()
        {
            // Act
            IConfigurationRoot configuration = ConfigurationHelper.CreateConfiguration();

            // Assert
            Assert.IsNotNull(configuration);
        }

        [Test]
        public void CreateBuilder_Should_Return_Builder_Instance()
        {
            // Act
            IConfigurationBuilder builder = ConfigurationHelper.CreateBuilder();

            // Assert
            Assert.IsNotNull(builder);
        }
    }
}
