using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Services.Tests
{
    [TestFixture]
    public class StorageLocationDataServiceTests
    {
        private StorageLocationDataService<StorageLocation> _storageLocationDataService;
        private Mock<BoschContext> _mockBoschContext;
        private Mock<ILoggingService> _mockLoggingService;

        [SetUp]
        public void Setup()
        {
            _mockBoschContext = new Mock<BoschContext>();
            _mockLoggingService = new Mock<ILoggingService>();

            _storageLocationDataService = new StorageLocationDataService<StorageLocation>(_mockBoschContext.Object, _mockLoggingService.Object);
        }

        [Test]
        public async Task GetAllQueryable_Should_ReturnData_When_ValidParametersProvided()
        {
            // Arrange
            var storageLocations = new List<StorageLocation> { new StorageLocation(), new StorageLocation() };
            var mockDbSet = CreateMockDbSet(storageLocations.AsQueryable());
            _mockBoschContext.Setup(c => c.Set<StorageLocation>()).Returns(mockDbSet.Object);

            // Act
            var result = await _storageLocationDataService.GetAllQueryable(1, 10, "PropertyName", true);

            // Assert
            Assert.AreEqual(ResponseCode.Success, result.Code);
            Assert.AreEqual(storageLocations, result.Data);
        }

        [Test]
        public async Task GetAllQueryable_Should_ReturnError_When_DbUpdateExceptionOccurs()
        {
            // Arrange
            _mockBoschContext.Setup(c => c.Set<StorageLocation>()).Throws<DbUpdateException>();

            // Act
            var result = await _storageLocationDataService.GetAllQueryable(1, 10, "PropertyName", true);

            // Assert
            Assert.AreEqual(ResponseCode.Error, result.Code);
            Assert.AreEqual("GetAllQueryable() failed", result.ErrorMessage);
        }

        [Test]
        public async Task MaxEntries_Should_ReturnCount_When_ValidParametersProvided()
        {
            // Arrange
            var storageLocations = new List<StorageLocation> { new StorageLocation(), new StorageLocation() };
            var mockDbSet = CreateMockDbSet(storageLocations.AsQueryable());
            _mockBoschContext.Setup(c => c.Set<StorageLocation>()).Returns(mockDbSet.Object);

            // Act
            var result = await _storageLocationDataService.MaxEntries();

            // Assert
            Assert.AreEqual(ResponseCode.Success, result.Code);
            Assert.AreEqual(storageLocations.Count, result.Data);
        }

        [Test]
        public async Task MaxEntries_Should_ReturnError_When_DbUpdateExceptionOccurs()
        {
            // Arrange
            _mockBoschContext.Setup(c => c.Set<StorageLocation>()).Throws<DbUpdateException>();

            // Act
            var result = await _storageLocationDataService.MaxEntries();

            // Assert
            Assert.AreEqual(ResponseCode.Error, result.Code);
            Assert.AreEqual("MaxEntries() failed", result.ErrorMessage);
        }

        [Test]
        public async Task MaxEntriesFiltered_Should_ReturnCount_When_ValidParametersProvided()
        {
            // Arrange
            var storageLocations = new List<StorageLocation> { new StorageLocation(), new StorageLocation() };
            var mockDbSet = CreateMockDbSet(storageLocations.AsQueryable());
            _mockBoschContext.Setup(c => c.Set<StorageLocation>()).Returns(mockDbSet.Object);

            // Act
            Expression<Func<StorageLocation, bool>> where = sl => sl.Id == 1;
            var result = await _storageLocationDataService.MaxEntriesFiltered(where);

            // Assert
            Assert.AreEqual(ResponseCode.Success, result.Code);
            Assert.AreEqual(1, result.Data); // Assuming only one storage location matches the filter
        }

        [Test]
        public async Task MaxEntriesFiltered_Should_ReturnError_When_DbUpdateExceptionOccurs()
        {
            // Arrange
            _mockBoschContext.Setup(c => c.Set<StorageLocation>()).Throws<DbUpdateException>();

            // Act
            Expression<Func<StorageLocation, bool>> where = sl => sl.Id == 1;
            var result = await _storageLocationDataService.MaxEntriesFiltered(where);

            // Assert
            Assert.AreEqual(ResponseCode.Error, result.Code);
            Assert.AreEqual("MaxEntries() failed", result.ErrorMessage);
        }

        // Additional test methods for other methods in StorageLocationDataService<T>...

        private Mock<DbSet<T>> CreateMockDbSet<T>(IQueryable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }
    }
}
