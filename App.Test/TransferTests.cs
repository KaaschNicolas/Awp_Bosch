using NUnit.Framework;
using App.Core.Models;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Models.Tests
{
    [TestFixture]
    public class TransferTests
    {
        [Test]
        public void Transfer_Comment_DefaultValue_Success()
        {
            // Arrange
            Transfer transfer = new Transfer();

            // Act

            // Assert
            Assert.IsNull(transfer.Comment);
        }

        [Test]
        public void Transfer_StorageLocationId_Required_Success()
        {
            // Arrange
            Transfer transfer = new Transfer();

            // Act

            // Assert
            Assert.That(transfer.StorageLocationId, Is.EqualTo(0));
        }

        [Test]
        public void Transfer_StorageLocation_Required_Success()
        {
            // Arrange
            Transfer transfer = new Transfer();

            // Act

            // Assert
            Assert.IsNull(transfer.StorageLocation);
        }

        [Test]
        public void Transfer_NotedById_Required_Success()
        {
            // Arrange
            Transfer transfer = new Transfer();

            // Act

            // Assert
            Assert.That(transfer.NotedById, Is.EqualTo(0));
        }

        [Test]
        public void Transfer_NotedBy_DefaultValue_Success()
        {
            // Arrange
            Transfer transfer = new Transfer();

            // Act

            // Assert
            Assert.IsNull(transfer.NotedBy);
        }

        [Test]
        public void Transfer_PcbId_DefaultValue_Success()
        {
            // Arrange
            Transfer transfer = new Transfer();

            // Act

            // Assert
            Assert.That(transfer.PcbId, Is.EqualTo(0));
        }

        [Test]
        public void Transfer_Pcb_Required_Success()
        {
            // Arrange
            Transfer transfer = new Transfer();

            // Act

            // Assert
            Assert.IsNull(transfer.Pcb);
        }
    }
}
