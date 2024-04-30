namespace Shared.Common.Logging.Interfaces;

public interface IEventLoggerService<T>
{
    void LogInformation(string message);
    void LogError(string message);
    void LogCritical(string message);
    void LogTrace(string message);
    void LogWarning(string message);
}