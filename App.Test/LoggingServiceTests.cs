using App.Core.DataAccess;
using App.Core.Helpers;
using App.Core.Models;
using App.Core.Services;
using App.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace App.Core.Tests.Services
{
    public class LoggingServiceTests
    {
        private readonly ILoggingService _loggingService;
        private readonly Mock<BoschContext> _boschContextMock;
        private readonly Mock<ILogger<LoggingService>> _loggerMock;

        public LoggingServiceTests()
        {
            _boschContextMock = new Mock<BoschContext>();
            _loggerMock = new Mock<ILogger<LoggingService>>();
            _loggingService = new LoggingService(_boschContextMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void Log_Should_Call_Logger_Log_Method()
        {
            // Arrange
            LogLevel logLevel = LogLevel.Information;
            string message = "Log message";

            // Act
            _loggingService.Log(logLevel, message);

            // Assert
            _loggerMock.Verify(logger => logger.Log(logLevel, message), Times.Once);
        }

        [Fact]
        public void Log_With_Object_Should_Call_Logger_Log_Method()
        {
            // Arrange
            LogLevel logLevel = LogLevel.Information;
            string message = "Log message";
            var obj = new { Property1 = "Value1", Property2 = "Value2" };

            // Act
            _loggingService.Log(logLevel, message, obj);

            // Assert
            string expectedMessage = $"{message} {obj.PropertiesToString()}";
            _loggerMock.Verify(logger => logger.Log(logLevel, expectedMessage), Times.Once);
        }

        [Fact]
        public void Audit_Should_Log_And_Save_Audit_Entry()
        {
            // Arrange
            LogLevel logLevel = LogLevel.Information;
            string message = "Audit message";
            var user = new User { Id = 1, Name = "John" };
            var auditEntry = new AuditEntry
            {
                Message = message,
                User = user,
                Level = logLevel.ToString()
            };

            // Act
            _loggingService.Audit(logLevel, message, user);

            // Assert
            _loggerMock.Verify(logger => logger.Log(logLevel, message), Times.Once);
            _boschContextMock.Verify(context => context.AuditEntries.Add(auditEntry), Times.Once);
            _boschContextMock.Verify(context => context.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Audit_With_Exception_Should_Log_And_Save_Audit_Entry_With_Exception()
        {
            // Arrange
            LogLevel logLevel = LogLevel.Error;
            string message = "Audit message";
            var user = new User { Id = 1, Name = "John" };
            var exception = new System.Exception("Exception message");
            var auditEntry = new AuditEntry
            {
                Message = message,
                User = user,
                Level = logLevel.ToString(),
                Exception = exception.Message
            };

            // Act
            _loggingService.Audit(logLevel, message, user, exception);

            // Assert
            _loggerMock.Verify(logger => logger.Log(logLevel, message), Times.Once);
            _boschContextMock.Verify(context => context.AuditEntries.Add(auditEntry), Times.Once);
            _boschContextMock.Verify(context => context.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Audit_With_Exception_And_Object_Should_Log_And_Save_Audit_Entry_With_Exception_And_Object()
        {
            // Arrange
            LogLevel logLevel = LogLevel.Error;
            string message = "Audit message";
            var user = new User { Id = 1, Name = "John" };
            var exception = new System.Exception("Exception message");
            var obj = new { Property1 = "Value1", Property2 = "Value2" };
            var auditEntry = new AuditEntry
            {
                Message = $"{message}: {obj.PropertiesToString()}",
                User = user,
                Level = logLevel.ToString(),
                Exception = exception.Message
            };

            // Act
            _loggingService.Audit(logLevel, message, user, exception, obj);

            // Assert
            _loggerMock.Verify(logger => logger.Log(logLevel, It.IsAny<string>()), Times.Once);
            _boschContextMock.Verify(context => context.AuditEntries.Add(auditEntry), Times.Once);
            _boschContextMock.Verify(context => context.SaveChanges(), Times.Once);
        }
    }
}
