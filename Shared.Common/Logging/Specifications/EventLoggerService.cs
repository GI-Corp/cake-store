using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Shared.Common.Logging.Interfaces;

namespace Shared.Common.Logging.Specifications;

public class EventLoggerService<T> : IEventLoggerService<T>
{
    private readonly ILogger<T> _logger;

    public EventLoggerService(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogError(string message)
    {
        _logger.LogError(message);
        LogEvent(message, "ERROR");
    }

    public void LogInformation(string message)
    {
        _logger.LogInformation(message);
        LogEvent(message);
    }

    public void LogCritical(string message)
    {
        _logger.LogCritical(message);
        LogEvent(message, "CRITICAL");
    }

    public void LogTrace(string message)
    {
        _logger.LogTrace(message);
        LogEvent(message, "TRACE");
    }

    public void LogWarning(string message)
    {
        _logger.LogWarning(message);
        LogEvent(message, "WARNING");
    }

    protected void LogEvent(string message, string logLevel = "INFO")
    {
        var currentActivity = Activity.Current;
        currentActivity?.AddEvent(new ActivityEvent($"{logLevel} {nameof(T)}: {message}"));
    }
}