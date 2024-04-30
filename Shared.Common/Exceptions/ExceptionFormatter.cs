using Shared.Domain.Entities.Exceptions;

namespace Shared.Common.Exceptions;

public class ExceptionFormatter
{
    private readonly Dictionary<Type, Func<Exception, string>> _exceptionFormatters =
        new()
        {
            { typeof(BusinessFlowException), FormatBusinessFlowException },
            { typeof(BaseException), FormatBaseException }
        };

    public string FormatExceptionMessage(Exception exception)
    {
        return _exceptionFormatters.TryGetValue(exception.GetType().BaseType!, out var formatter)
            ? formatter(exception)
            : FormatDefaultException(exception);
    }

    private static string FormatBusinessFlowException(Exception exception)
    {
        if (exception is BusinessFlowException businessFlowException)
            return
                $@"BusinessFlowExceptionType ({businessFlowException.Code}): {businessFlowException.Message}";

        return string.Empty;
    }

    private static string FormatBaseException(Exception exception)
    {
        if (exception is BaseException baseException)
            return $@"BaseExceptionType ({baseException.Code}): {baseException.Message}";

        return string.Empty;
    }

    private static string FormatDefaultException(Exception exception)
    {
        var thrownAt = exception.TargetSite!.ReflectedType!.Name;
        return $@"Exception at ({exception.Source} {thrownAt}), Details: {exception.Message}";
    }
}