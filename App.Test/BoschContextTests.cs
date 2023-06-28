using App.Core.DataAccess;
using App.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App.Core.Tests.DataAccess
{
    [TestClass]
    public class BoschContextTests
    {
        [TestMethod]
        public void BoschContext_Constructor_DefaultConstructor_ShouldInitializeContext()
        {
            // Arrange
            var context = new BoschContext();

            // Act and Assert
            Assert.IsNotNull(context);
        }

        [TestMethod]
        public void BoschContext_Constructor_OptionsConstructor_ShouldInitializeContext()
        {
            // Arrange
            var options = new DbContextOptions<BoschContext>();
            var context = new BoschContext(options);

            // Act and Assert
            Assert.IsNotNull(context);
        }

        [TestMethod]
        public void BoschContext_DbSet_Comments_ShouldReturnDbSet()
        {
            // Arrange
            var options = new DbContextOptions<BoschContext>();
            var context = new BoschContext(options);

            // Act
            var comments = context.Comments;

            // Assert
            Assert.IsNotNull(comments);
        }

        // Repeat similar tests for other DbSet properties (Diagnoses, ErrorTypes, Devices, StorageLocations, Pcbs, PcbTypes, Users, Transfers, AuditEntries)

        [TestMethod]
        public void BoschContext_OnConfiguring_ShouldSetSqlServerConnectionString()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BoschContext>()
                .UseSqlServer("TestConnectionString")
                .Options;
            var context = new BoschContext(options);

            // Act
            var connectionString = context.Database.GetDbConnection().ConnectionString;

            // Assert
            Assert.AreEqual("TestConnectionString", connectionString);
        }

        [TestMethod]
        public void BoschContext_SaveChanges_AddedEntity_ShouldSetCreatedDate()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BoschContext>()
                .Options;

            using (var context = new BoschContext(options))
            {
                var entity = new Device(); 

                // Act
                context.Add(entity);
                context.SaveChanges();

                // Assert
                Assert.AreNotEqual(DateTime.MinValue, entity.CreatedDate);
            }
        }

        [TestMethod]
        public void BoschContext_SaveChangesAsync_AddedEntity_ShouldSetCreatedDate()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BoschContext>()
                .Options;

            using (var context = new BoschContext(options))
            {
                var entity = new Device();

                // Act
                context.Add(entity);
                context.SaveChangesAsync().GetAwaiter().GetResult();

                // Assert
                Assert.AreNotEqual(DateTime.MinValue, entity.CreatedDate);
            }
        }

    }
}
