using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using App.Core.Migrations;
using System;
using Assert = NUnit.Framework.Assert;
using App.Core.DataAccess;
using Microsoft.Extensions.Configuration;

namespace App.Core.Tests
{
    [TestFixture]
    public class MigrationTestsInit
    {
        //private DbContextOptions<AppDbContext> _dbContextOptions;

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<BoschContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("BoschContext"));

        [SetUp]
        public void Setup()
        {
            // Create an instance of DbContextOptions using the InMemory database provider
            //_dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            //    .UseInMemoryDatabase(databaseName: "TestDatabase")
            //    .Options;
            var dbContext = new BoschContext(optionsBuilder.Options);
        }

        [Test]
        public void CanApplyMigrations()
        {
            using (var dbContext = new AppDbContext(_dbContextOptions))
            {
                // Apply the migration
                var migration = new Init();
                migration.Up(dbContext.Database);

                // Assert that the necessary tables have been created
                Assert.That(dbContext.Database.HasTable("AuditEntries"));
                Assert.That(dbContext.Database.HasTable("Devices"));
                Assert.That(dbContext.Database.HasTable("Diagnoses"));
                Assert.That(dbContext.Database.HasTable("ErrorTypes"));
                Assert.That(dbContext.Database.HasTable("PcbTypes"));
                Assert.That(dbContext.Database.HasTable("StorageLocations"));
                Assert.That(dbContext.Database.HasTable("Users"));
                Assert.That(dbContext.Database.HasTable("Comments"));
                Assert.That(dbContext.Database.HasTable("Pcbs"));
                Assert.That(dbContext.Database.HasTable("ErrorTypePcb"));
                Assert.That(dbContext.Database.HasTable("Transfers"));
            }
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database after each test
            using (var dbContext = new AppDbContext(_dbContextOptions))
            {
                dbContext.Database.EnsureDeleted();
            }
        }
    }
}
