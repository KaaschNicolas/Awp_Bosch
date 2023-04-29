using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;
using Microsoft.Extensions.Logging;

namespace App.Core.Services.Interfaces;
public interface ILoggingService
{
    public void Log(LogLevel logLevel, string message);
    public void Log(LogLevel logLevel, string message, object obj);
    public void Audit(LogLevel logLevel, string message, User user);
    public void Audit(LogLevel logLevel, string message, User user, Exception exception);
    public void Audit(LogLevel logLevel, string message, User user, Exception? exception, object obj);
}
