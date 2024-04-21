using CakeStore.Application.Interfaces;
using CakeStoreApp.Extensions;
using CakeStoreApp.Mappers.Reference;
using Shared.Common.Logging.Interfaces;
using Shared.Common.Middlewares;
using Shared.Domain.Entities.Exceptions;
using Shared.Presentation.ViewModels.Exceptions;

namespace CakeStoreApp.Middlewares;

public class CakeStoreExceptionHandlerMiddleware : BaseErrorHandlerMiddleware<CakeStoreExceptionHandlerMiddleware>
{
    private readonly IErrorService _errorService;
    private readonly IReferenceContainer _referenceContainer;

    public CakeStoreExceptionHandlerMiddleware(
        IEventLoggerService<CakeStoreExceptionHandlerMiddleware> logger,
        IErrorService errorService,
        IReferenceContainer referenceContainer
    ) : base(logger)
    {
        _errorService = errorService ?? throw new ArgumentNullException(nameof(errorService));
        _referenceContainer = referenceContainer ?? throw new ArgumentNullException(nameof(referenceContainer));
    }
    
    public override ErrorResponseViewModel GetBusinessFlowErrorResponse(BusinessFlowException exception,
        HttpContext context)
    {
        var languageId = context.Request.GetLanguageIdFromHeaders(_referenceContainer);
        var errorMessage = _errorService.Get(exception.Code, languageId);

        return new ErrorResponseViewModel
        {
            Code = exception.Code,
            Details = errorMessage,
            LanguageId = languageId
        };
    }
}