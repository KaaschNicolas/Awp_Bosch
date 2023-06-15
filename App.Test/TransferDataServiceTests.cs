using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Services.Tests
{
    [TestFixture]
    public class TransferDataServiceTests
    {
        private TransferDataService<Transfer> _transferDataService;
        private Mock<BoschContext> _mockBoschContext;
        private Mock<ILoggingService> _mockLoggingService;

        [SetUp]
        public void Setup()
        {
            _mockBoschContext = new Mock<BoschContext>();
            _mockLoggingService = new Mock<ILoggingService>();

            _transferDataService = new TransferDataService<Transfer>(_mockBoschContext.Object, _mockLoggingService.Object);
        }

        [Test]
        public async Task GetTransfersByPcb_Should_ReturnData_When_ValidPcbIdProvided()
        {
            // Arrange
            var transfers = new List<Transfer> { new Transfer(), new Transfer() };
            var mockDbSet = CreateMockDbSet(transfers.AsQueryable());
            _mockBoschContext.Setup(c => c.Set<Transfer>()).Returns(mockDbSet.Object);

            // Act
            var result = await _transferDataService.GetTransfersByPcb(1);

            // Assert
            Assert.AreEqual(ResponseCode.Success, result.Code);
            Assert.AreEqual(transfers, result.Data);
        }

        [Test]
        public async Task GetTransfersByPcb_Should_ReturnError_When_DbUpdateExceptionOccurs()
        {
            // Arrange
            _mockBoschContext.Setup(c => c.Set<Transfer>()).Throws<DbUpdateException>();

            // Act
            var result = await _transferDataService.GetTransfersByPcb(1);

            // Assert
            Assert.AreEqual(ResponseCode.Error, result.Code);
            Assert.AreEqual("GetTransfersByPcb() failed", result.ErrorMessage);
        }
        /*
        [Test]
        public async Task CreateTransfer_Should_ReturnSuccessResponse_When_ValidTransferAndOptionalDiagnoseIdProvided()
        {
            // Arrange
            var transfer = new Transfer();
            var mockDbSet = CreateMockDbSet<Transfer>(Enumerable.Empty<Transfer>().AsQueryable());
            var mockPcbDbSet = CreateMockDbSet<Pcb>(Enumerable.Empty<Pcb>().AsQueryable());
            var mockStorageLocationDbSet = CreateMockDbSet<StorageLocation>(Enumerable.Empty<StorageLocation>().AsQueryable());
            _mockBoschContext.Setup(c => c.Set<Transfer>()).Returns(mockDbSet.Object);
            _mockBoschContext.Setup(c => c.Set<Pcb>()).Returns(mockPcbDbSet.Object);
            _mockBoschContext.Setup(c => c.Set<StorageLocation>()).Returns(mockStorageLocationDbSet.Object);
            _mockBoschContext.Setup(c => c.SaveChangesAsync()).Returns(Task.FromResult(0));

            // Act
            var result = await _transferDataService.CreateTransfer(transfer, 1);

            // Assert
            Assert.AreEqual(ResponseCode.Success, result.Code);
            Assert.AreEqual(transfer, result.Data);
            _mockBoschContext.Verify(c => c.Set<Transfer>().AddAsync(transfer), Times.Once);
            _mockBoschContext.Verify(c => c.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task CreateTransfer_Should_ReturnError_When_ExceptionOccurs()
        {
            // Arrange
            var transfer = new Transfer();
            var mockDbSet = CreateMockDbSet<Transfer>(Enumerable.Empty<Transfer>().AsQueryable());
            _mockBoschContext.Setup(c => c.Set<Transfer>()).Returns(mockDbSet.Object);
            _mockBoschContext.Setup(c => c.SaveChangesAsync()).Throws<Exception>();

            // Act
            var result = await _transferDataService.CreateTransfer(transfer, 1);

            // Assert
            Assert.AreEqual(ResponseCode.Error, result.Code);
            Assert.AreEqual($"Fehler beim Erstellen von {typeof(Transfer)}", result.ErrorMessage);
        }
        */
        [Test]
        public async Task GetAllGroupedByStorageLocation_Should_ReturnData_When_Called()
        {
            // Arrange
            var transfers = new List<Transfer> { new Transfer(), new Transfer() };
            var mockDbSet = CreateMockDbSet(transfers.AsQueryable());
            _mockBoschContext.Setup(c => c.Set<Transfer>()).Returns(mockDbSet.Object);

            // Act
            var result = await _transferDataService.GetAllGroupedByStorageLocation();

            // Assert
            Assert.AreEqual(ResponseCode.Success, result.Code);
            Assert.AreEqual(transfers.GroupBy(t => t.StorageLocationId).ToList(), result.Data);
        }

        [Test]
        public async Task GetAllGroupedByStorageLocation_Should_ReturnError_When_DbUpdateExceptionOccurs()
        {
            // Arrange
            _mockBoschContext.Setup(c => c.Set<Transfer>()).Throws<DbUpdateException>();

            // Act
            var result = await _transferDataService.GetAllGroupedByStorageLocation();

            // Assert
            Assert.AreEqual(ResponseCode.Error, result.Code);
            Assert.AreEqual("GetAllGroupedByStorageLocation() failed", result.ErrorMessage);
        }

        [Test]
        public async Task GetAllEager_Should_ReturnData_When_Called()
        {
            // Arrange
            var transfers = new List<Transfer> { new Transfer { DeletedDate = DateTime.MinValue }, new Transfer() };
            var mockDbSet = CreateMockDbSet(transfers.AsQueryable());
            _mockBoschContext.Setup(c => c.Set<Transfer>()).Returns(mockDbSet.Object);

            // Act
            var result = await _transferDataService.GetAllEager();

            // Assert
            Assert.AreEqual(ResponseCode.Success, result.Code);
            Assert.AreEqual(transfers.Where(t => t.DeletedDate == DateTime.MinValue).ToList(), result.Data);
        }

        [Test]
        public async Task GetAllEager_Should_ReturnError_When_DbUpdateExceptionOccurs()
        {
            // Arrange
            _mockBoschContext.Setup(c => c.Set<Transfer>()).Throws<DbUpdateException>();

            // Act
            var result = await _transferDataService.GetAllEager();

            // Assert
            Assert.AreEqual(ResponseCode.Success, result.Code);
            Assert.AreEqual("GetAllEager() failed", result.ErrorMessage);
        }

        // Additional test methods for other methods in TransferDataService<T>...

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
