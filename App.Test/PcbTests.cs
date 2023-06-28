using App.Core.Models;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Models.Tests
{
    [TestFixture]
    public class PcbTests
    {
        [Test]
        public void Pcb_Properties_Are_Set_Correctly()
        {
            // Arrange
            var pcb = new Pcb();

            // Act
            pcb.SerialNumber = "123456";
            pcb.Restriction = new Device();
            pcb.ErrorDescription = "Sample error description";
            pcb.ErrorTypes = new List<ErrorType>();
            pcb.Finalized = true;
            pcb.PcbTypeId = 1;
            pcb.PcbType = new PcbType();
            pcb.Comment = new Comment();
            pcb.Transfers = new List<Transfer>();
            pcb.Diagnose = new Diagnose();
            pcb.DiagnoseId = 2;

            // Assert
            Assert.AreEqual("123456", pcb.SerialNumber);
            Assert.IsNotNull(pcb.Restriction);
            Assert.IsInstanceOf<Device>(pcb.Restriction);
            Assert.AreEqual("Sample error description", pcb.ErrorDescription);
            Assert.IsNotNull(pcb.ErrorTypes);
            Assert.IsInstanceOf<List<ErrorType>>(pcb.ErrorTypes);
            Assert.IsTrue(pcb.Finalized);
            Assert.AreEqual(1, pcb.PcbTypeId);
            Assert.IsNotNull(pcb.PcbType);
            Assert.IsInstanceOf<PcbType>(pcb.PcbType);
            Assert.IsNotNull(pcb.Comment);
            Assert.IsInstanceOf<Comment>(pcb.Comment);
            Assert.IsNotNull(pcb.Transfers);
            Assert.IsInstanceOf<List<Transfer>>(pcb.Transfers);
            Assert.IsNotNull(pcb.Diagnose);
            Assert.IsInstanceOf<Diagnose>(pcb.Diagnose);
            Assert.AreEqual(2, pcb.DiagnoseId);
        }
    }
}
