using App.Core.Models;
using NUnit.Framework;
using System.Collections.Generic;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Models.Tests
{
    [TestFixture]
    public class PcbTypeTests
    {
        [Test]
        public void PcbType_Properties_Are_Set_Correctly()
        {
            // Arrange
            var pcbType = new PcbType();

            // Act
            pcbType.PcbPartNumber = "ABC123";
            pcbType.Description = "Sample PCB Type";
            pcbType.MaxTransfer = 10;

            // Assert
            Assert.AreEqual("ABC123", pcbType.PcbPartNumber);
            Assert.AreEqual("Sample PCB Type", pcbType.Description);
            Assert.AreEqual(10, pcbType.MaxTransfer);
        }

        [Test]
        public void PcbType_Pcbs_Initialized_Correctly()
        {
            // Arrange
            var pcbType = new PcbType();

            // Act
            pcbType.Pcbs = new List<Pcb>();

            // Assert
            Assert.IsNotNull(pcbType.Pcbs);
            Assert.IsInstanceOf<List<Pcb>>(pcbType.Pcbs);
        }
    }
}
