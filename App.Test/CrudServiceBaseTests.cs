using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services.Base;
using App.Core.Services.Interfaces;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace App.Core.Tests.Services
{
    public class CrudServiceBaseTests
    {
        private readonly Mock<BoschContext> _mockContext;
        private readonly Mock<ILoggingService> _mockLoggingService;

        public CrudServiceBaseTests()
        {
            _mockContext = new Mock<BoschContext>();
            _mockLoggingService = new Mock<ILoggingService>();
        }

        [Fact]
        public async Task Create_ValidEntity_ReturnsSuccessResponseWithCreatedEntity()
        {
            // Arrange
            var entity = new Mock<BaseEntity>().Object;
            var service = new TestCrudService(_mockContext.Object, _mockLoggingService.Object);

            // Act
            var result = await service.Create(entity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ResponseCode.Success, result.Code);
            Assert.Same(entity, result.Data);
        }

        

        [Fact]
        public async Task Update_ValidEntity_ReturnsSuccessResponseWithUpdatedEntity()
        {
            // Arrange
            var id = 1;
            var entity = new Mock<BaseEntity> { CallBase = true }.Object;
            var service = new TestCrudService(_mockContext.Object, _mockLoggingService.Object);

            // Act
            var result = await service.Update(id, entity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ResponseCode.Success, result.Code);
            Assert.Same(entity, result.Data);
        }

        [Fact]
        public async Task Delete_ValidEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new Mock<BaseEntity> { CallBase = true }.Object;
            var service = new TestCrudService(_mockContext.Object, _mockLoggingService.Object);

            // Act
            var result = await service.Delete(entity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ResponseCode.Success, result.Code);
            Assert.Equal($"{typeof(BaseEntity)} erfolgreich gelöscht.", result.ErrorMessage);
        }

        

        // Test implementation of CrudServiceBase<T> for testing
        private class TestCrudService : CrudServiceBase<BaseEntity>
        {
            public TestCrudService(BoschContext boschContext, ILoggingService loggingService)
                : base(boschContext, loggingService)
            {
            }
        }
    }
}
