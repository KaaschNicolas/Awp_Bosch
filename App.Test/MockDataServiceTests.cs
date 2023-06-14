using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace App.Core.Tests.Services
{
    public class MockDataServiceTests
    {
        private readonly IMockDataService _mockDataService;
        private readonly Mock<BoschContext> _boschContextMock;

        public MockDataServiceTests()
        {
            _boschContextMock = new Mock<BoschContext>();
            _mockDataService = new MockDataService(_boschContextMock.Object);
        }
        /*
        [Fact]
        public async Task SeedMockData_Should_Seed_ErrorType_Data_If_No_Records_Exist()
        {
            // Arrange
            _boschContextMock.Setup(context => context.ErrorTypes.CountAsync()).ReturnsAsync(0);
            var mockData = new List<ErrorType> { /* populate with test data / };
            var json = JsonSerializer.Serialize(mockData);
            _boschContextMock.Setup(context => context.AddRange(mockData));
            _boschContextMock.Setup(context => context.SaveChanges());

            // Act
            _mockDataService.SeedMockData();

            // Assert
            _boschContextMock.Verify(context => context.ErrorTypes.CountAsync(), Times.Once);
            _boschContextMock.Verify(context => context.AddRange(mockData), Times.Once);
            _boschContextMock.Verify(context => context.SaveChanges(), Times.Once);
        }
        */

        [Fact]
        public void SeedFromExcel_Should_Do_Something()
        {
            // Arrange
            string path = "mockExcelPath";
            // Mock the necessary setup for Excel reading and saving

            // Act
            _mockDataService.SeedFromExcel(path);

            // Assert
            // Add assertions for the expected behavior
        }
    }
}
