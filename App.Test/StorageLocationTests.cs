using NUnit.Framework;
using App.Core.Models;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Models.Tests
{
    [TestFixture]
    public class StorageLocationTests
    {
        [Test]
        public void StorageLocation_StorageName_Required_Success()
        {
            // Arrange
            StorageLocation storageLocation = new StorageLocation();

            // Act

            // Assert
            Assert.That(storageLocation.StorageName, Is.Null);
        }

        [Test]
        public void StorageLocation_DwellTimeYellow_DefaultValue_Success()
        {
            // Arrange
            StorageLocation storageLocation = new StorageLocation();

            // Act

            // Assert
            Assert.IsNull(storageLocation.DwellTimeYellow);
        }

        [Test]
        public void StorageLocation_DwellTimeRed_DefaultValue_Success()
        {
            // Arrange
            StorageLocation storageLocation = new StorageLocation();

            // Act

            // Assert
            Assert.IsNull(storageLocation.DwellTimeRed);
        }

        [Test]
        public void StorageLocation_IsFinalDestination_DefaultValue_Success()
        {
            // Arrange
            StorageLocation storageLocation = new StorageLocation();

            // Act

            // Assert
            Assert.IsFalse(storageLocation.IsFinalDestination);
        }

        [Test]
        public void StorageLocation_Transfers_DefaultValue_Success()
        {
            // Arrange
            StorageLocation storageLocation = new StorageLocation();

            // Act

            // Assert
            Assert.That(storageLocation.Transfers, Is.Null);
        }

        [Test]
        public void StorageLocation_Equals_Success()
        {
            // Arrange
            StorageLocation storageLocation1 = new StorageLocation { Id = 1, StorageName = "Location1" };
            StorageLocation storageLocation2 = new StorageLocation { Id = 1, StorageName = "Location1" };
            StorageLocation storageLocation3 = new StorageLocation { Id = 2, StorageName = "Location2" };

            // Act

            // Assert
            Assert.IsTrue(storageLocation1.Equals(storageLocation2));
            Assert.IsFalse(storageLocation1.Equals(storageLocation3));
        }

        [Test]
        public void StorageLocation_GetHashCode_Success()
        {
            // Arrange
            StorageLocation storageLocation1 = new StorageLocation { Id = 1, StorageName = "Location1" };
            StorageLocation storageLocation2 = new StorageLocation { Id = 1, StorageName = "Location1" };
            StorageLocation storageLocation3 = new StorageLocation { Id = 2, StorageName = "Location2" };

            // Act

            // Assert
            Assert.AreEqual(storageLocation1.GetHashCode(), storageLocation2.GetHashCode());
            Assert.AreNotEqual(storageLocation1.GetHashCode(), storageLocation3.GetHashCode());
        }
    }
}
