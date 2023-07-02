using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services;
using App.Core.Services.Base;
using App.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace App.Core.Tests.Services
{
    public class CrudServiceTests
    {
        private readonly Mock<BoschContext> _mockBoschContext;
        private readonly Mock<ILoggingService> _mockLoggingService;
        private readonly ICrudService<Foo> _crudService;

        public CrudServiceTests()
        {
            _mockBoschContext = new Mock<BoschContext>();
            _mockLoggingService = new Mock<ILoggingService>();
            _crudService = new CrudService<Foo>(_mockBoschContext.Object, _mockLoggingService.Object);
        }
       
        [Fact]
        public async Task GetById_Should_Return_Response_With_Entity_By_Id()
        {
            // Arrange
            var entityId = 1;
            var entity = new Foo { Id = entityId, Name = "Test Foo" };

            _mockBoschContext.Setup(c => c.FindAsync<Foo>(entityId)).ReturnsAsync(entity);

            // Act
            var response = await _crudService.GetById(entityId);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(entity, response.Data);
        }

        [Fact]
        public async Task Dispose_Should_Call_Dispose_On_BoschContext()
        {
            // Act
            await _crudService.Dispose();

            // Assert
            _mockBoschContext.Verify(c => c.Dispose(), Times.Once);
        }
    }

    public class Foo : BaseEntity
    {
        public string Name { get; set; }
    }
}
