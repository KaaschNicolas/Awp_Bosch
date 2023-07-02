using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class PcbDataServiceTests
    {
        private PcbDataService<Pcb> _pcbDataService;
        private Mock<BoschContext> _mockBoschContext;
        private Mock<ILoggingService> _mockLoggingService;

        [SetUp]
        public void Setup()
        {
            _mockBoschContext = new Mock<BoschContext>();
            _mockLoggingService = new Mock<ILoggingService>();

            _pcbDataService = new PcbDataService<Pcb>(_mockBoschContext.Object, _mockLoggingService.Object);
        }

        [Test]
        public async Task GetAllQueryable_Should_ReturnData_When_ValidParametersProvided()
        {
            // Arrange
            var pcbs = new List<Pcb> { new Pcb(), new Pcb() };
            var mockDbSet = CreateMockDbSet(pcbs.AsQueryable());
            _mockBoschContext.Setup(c => c.Set<Pcb>()).Returns(mockDbSet.Object);

            // Act
            var result = await _pcbDataService.GetAllQueryable(1, 10, "PropertyName", true);

            // Assert
            Assert.AreEqual(ResponseCode.Success, result.Code);
            Assert.AreEqual(pcbs, result.Data);
        }

        [Test]
        public async Task GetAllQueryable_Should_ReturnError_When_DbUpdateExceptionOccurs()
        {
            // Arrange
            _mockBoschContext.Setup(c => c.Set<Pcb>()).Throws<DbUpdateException>();

            // Act
            var result = await _pcbDataService.GetAllQueryable(1, 10, "PropertyName", true);

            // Assert
            Assert.AreEqual(ResponseCode.Error, result.Code);
            Assert.AreEqual("GetAllQueryable() failed", result.ErrorMessage);
        }

        [Test]
        public async Task MaxEntries_Should_ReturnCount_When_ValidParametersProvided()
        {
            // Arrange
            var pcbs = new List<Pcb> { new Pcb(), new Pcb() };
            var mockDbSet = CreateMockDbSet(pcbs.AsQueryable());
            _mockBoschContext.Setup(c => c.Set<Pcb>()).Returns(mockDbSet.Object);

            // Act
            var result = await _pcbDataService.MaxEntries();

            // Assert
            Assert.AreEqual(ResponseCode.Success, result.Code);
            Assert.AreEqual(pcbs.Count, result.Data);
        }

        [Test]
        public async Task MaxEntries_Should_ReturnError_When_DbUpdateExceptionOccurs()
        {
            // Arrange
            _mockBoschContext.Setup(c => c.Set<Pcb>()).Throws<DbUpdateException>();

            // Act
            var result = await _pcbDataService.MaxEntries();

            // Assert
            Assert.AreEqual(ResponseCode.Error, result.Code);
            Assert.AreEqual("MaxEntries() failed", result.ErrorMessage);
        }

        [Test]
        public async Task MaxEntriesFiltered_Should_ReturnCount_When_ValidParametersProvided()
        {
            // Arrange
            var pcbs = new List<Pcb> { new Pcb(), new Pcb() };
            var mockDbSet = CreateMockDbSet(pcbs.AsQueryable());
            _mockBoschContext.Setup(c => c.Set<Pcb>()).Returns(mockDbSet.Object);

            // Act
            Expression<Func<Pcb, bool>> filter = p => p.Id == 1;
            var result = await _pcbDataService.MaxEntriesFiltered(filter);

            // Assert
            Assert.AreEqual(ResponseCode.Success, result.Code);
            Assert.AreEqual(pcbs.Count, result.Data);
        }

        [Test]
        public async Task MaxEntriesFiltered_Should_ReturnError_When_DbUpdateExceptionOccurs()
        {
            // Arrange
            _mockBoschContext.Setup(c => c.Set<Pcb>()).Throws<DbUpdateException>();

            // Act
            Expression<Func<Pcb, bool>> filter = p => p.Id == 1;
            var result = await _pcbDataService.MaxEntriesFiltered(filter);

            // Assert
            Assert.AreEqual(ResponseCode.Error, result.Code);
            Assert.AreEqual("MaxEntriesFiltered() failed", result.ErrorMessage);
        }

        [Test]
        public async Task Delete_Should_ReturnSuccess_When_ValidIdProvided()
        {
            // Arrange
            var pcbId = 1;
            var pcb = new Pcb { Id = pcbId };
            var mockDbSet = CreateMockDbSet(new List<Pcb> { pcb }.AsQueryable());
            _mockBoschContext.Setup(c => c.Set<Pcb>()).Returns(mockDbSet.Object);

            // Act
            var result = await _pcbDataService.Delete(pcb);

            // Assert
            Assert.AreEqual(ResponseCode.Success, result.Code);
            _mockBoschContext.Verify(c => c.Remove(pcb), Times.Once);
            _mockBoschContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Test]
        public async Task Delete_Should_ReturnError_When_InvalidIdProvided()
        {
            // Arrange
            var pcbId = 1;
            var pcb = new Pcb { Id = pcbId };
            var mockDbSet = CreateMockDbSet(new List<Pcb>().AsQueryable());
            _mockBoschContext.Setup(c => c.Set<Pcb>()).Returns(mockDbSet.Object);

            // Act
            var result = await _pcbDataService.Delete(pcb);

            // Assert
            Assert.AreEqual(ResponseCode.Error, result.Code);
            Assert.AreEqual("Delete() failed", result.ErrorMessage);
            _mockBoschContext.Verify(c => c.Remove(It.IsAny<Pcb>()), Times.Never);
            _mockBoschContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

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
