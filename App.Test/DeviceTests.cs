using App.Core.Validator;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Models.Tests
{
    [TestFixture]
    public class DeviceTests
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
        public void Pcbs_GetAndSet_Successfully()
        {
            // Arrange
            Device device = new Device();
            List<Pcb> pcbs = new List<Pcb>
            {
                new Pcb(),
                new Pcb()
            };

            // Act
            device.Pcbs = pcbs;
            List<Pcb> result = device.Pcbs;

            // Assert
            Assert.AreEqual(pcbs, result);
        }
    }
}
