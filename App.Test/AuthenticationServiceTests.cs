using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace App.Core.Tests.Services
{
    public class AuthenticationServiceTests
    {
        [Fact]
        public void IsAuthenticated_Should_Return_True_When_User_Is_Authenticated()
        {
            // Arrange
            var mockContextOptions = new DbContextOptionsBuilder<BoschContext>().Options;
            var mockBoschContext = new Mock<BoschContext>(mockContextOptions);
            var mockDbSet = new Mock<DbSet<User>>();
            var user = new User { Id = 1, Name = "John Doe", AdUsername = "johndoe" };

            mockBoschContext.Setup(c => c.Users).Returns(mockDbSet.Object);
            mockDbSet.Setup(d => d.Add(It.IsAny<User>())).Verifiable();
            mockDbSet.Setup(d => d.FindAsync(It.IsAny<object[]>())).ReturnsAsync(user);

            var authenticationService = new AuthenticationService(mockBoschContext.Object);

            // Act
            bool isAuthenticated = authenticationService.IsAuthenticated;

            // Assert
            Assert.True(isAuthenticated);
        }

        [Fact]
        public void CurrentUser_Should_Return_User_Object()
        {
            // Arrange
            var mockContextOptions = new DbContextOptionsBuilder<BoschContext>().Options;
            var mockBoschContext = new Mock<BoschContext>(mockContextOptions);
            var mockDbSet = new Mock<DbSet<User>>();
            var user = new User { Id = 1, Name = "John Doe", AdUsername = "johndoe" };

            mockBoschContext.Setup(c => c.Users).Returns(mockDbSet.Object);
            mockDbSet.Setup(d => d.Add(It.IsAny<User>())).Verifiable();
            mockDbSet.Setup(d => d.FindAsync(It.IsAny<object[]>())).ReturnsAsync(user);

            var authenticationService = new AuthenticationService(mockBoschContext.Object);

            // Act
            User currentUser = authenticationService.CurrentUser;

            // Assert
            Assert.Equal(user, currentUser);
        }

        [Fact]
        public void IsDbActive_Should_Return_True_When_Db_Is_Active()
        {
            // Arrange
            var mockContextOptions = new DbContextOptionsBuilder<BoschContext>().Options;
            var mockBoschContext = new Mock<BoschContext>(mockContextOptions);

            var authenticationService = new AuthenticationService(mockBoschContext.Object);

            // Act
            bool isDbActive = authenticationService.IsDbActive;

            // Assert
            Assert.True(isDbActive);
        }
    }
}
