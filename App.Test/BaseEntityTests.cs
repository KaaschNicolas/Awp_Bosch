using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Models.Tests
{
    [TestFixture]
    public class BaseEntityTests
    {
        [Test]
        public void Id_SetAndGet_Successfully()
        {
            // Arrange
            int id = 1;
            BaseEntity entity = new AuditEntry();

            // Act
            entity.Id = id;
            int result = entity.Id;

            // Assert
            Assert.AreEqual(id, result);
        }

        [Test]
        public void CreatedDate_SetAndGet_Successfully()
        {
            // Arrange
            DateTime createdDate = new DateTime(2023, 1, 1);
            BaseEntity entity = new AuditEntry();

            // Act
            entity.CreatedDate = createdDate;
            DateTime result = entity.CreatedDate;

            // Assert
            Assert.AreEqual(createdDate, result);
        }

        [Test]
        public void DeletedDate_SetAndGet_Successfully()
        {
            // Arrange
            DateTime deletedDate = new DateTime(2023, 1, 1);
            BaseEntity entity = new AuditEntry();

            // Act
            entity.DeletedDate = deletedDate;
            DateTime result = entity.DeletedDate;

            // Assert
            Assert.AreEqual(deletedDate, result);
        }
    }
}
