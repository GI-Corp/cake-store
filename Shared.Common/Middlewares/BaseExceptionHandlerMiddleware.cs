using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Shared.Common.Authorization.Claims;
using Shared.Common.Exceptions;
using Shared.Common.Extensions.Request;
using Shared.Common.Helpers;
using Shared.Common.Logging.Interfaces;
using Shared.Domain.Entities.Exceptions;
using Shared.Presentation.ViewModels.Exceptions;

namespace Shared.Common.Middlewares;

public class BaseErrorHandlerMiddleware<T> : IMiddleware where T : class
{
    private readonly IEventLoggerService<T> _eventLogger;
    private readonly ExceptionFormatter _exceptionFormatter;

    protected BaseErrorHandlerMiddleware(IEventLoggerService<T> logger)
    {
        _eventLogger = logger;
        _exceptionFormatter = new ExceptionFormatter();
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            LogException(exception, context);
            HandleException(context, exception);
        }
    }

    public virtual ErrorResponseViewModel GetBusinessFlowErrorResponse(
        BusinessFlowException exception, HttpContext context)
    {
        var claims = new CommonClaimsPrincipal(context.User);
        var languageId = HeaderExtensions.GetDefaultLanguageId(claims);

        return new ErrorResponseViewModel
        {
            Code = exception.Code,
            Details = exception.Details,
            LanguageId = languageId
        };
    }

    private ErrorResponseViewModel GetDefaultErrorResponse()
    {
        const string languageId = Constants.Miscellaneous.DefaultExceptionLanguageId;
        return new ErrorResponseViewModel
        {
            Code = (short)HttpStatusCode.InternalServerError,
            Details = "Internal Server Error.",
            LanguageId = languageId
        };
    }

    private void LogException(Exception exception, HttpContext context)
    {
        var excLogMessage = _exceptionFormatter.FormatExceptionMessage(exception);
        _eventLogger.LogError(
            $"An unhandled exception at {context.Request.Path.Value}: {excLogMessage}");
    }

    private void HandleException(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = exception switch
        {
            BusinessFlowException businessFlowException => GetBusinessFlowErrorResponse(businessFlowException, context),
            _ => GetDefaultErrorResponse()
        };

        response.StatusCode = (short)HttpStatusCode.BadRequest;

        var result = JsonSerializer.Serialize(errorResponse);
        response.WriteAsync(result);
    }
}