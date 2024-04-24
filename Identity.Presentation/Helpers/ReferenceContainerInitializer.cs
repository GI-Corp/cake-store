using AutoMapper;
using Identity.Infrastructure.DAL.DbContexts;
using Identity.Presentation.Mappers.Reference;
using Microsoft.EntityFrameworkCore;
using Shared.Presentation.ViewModels.Reference;

namespace Identity.Presentation.Helpers;

public class ReferenceContainerInitializer
{
    private readonly IdentityContext _identityContext;
    private readonly IReferenceContainer _referenceContainer;
    private readonly ReferenceContext _referenceContext;
    private readonly IMapper _mapper;

    public ReferenceContainerInitializer(IdentityContext identityContext, ReferenceContext referenceContext, 
        IMapper mapper, IReferenceContainer referenceContainer)
    {
        _identityContext = identityContext ?? throw new ArgumentNullException(nameof(identityContext));
        _referenceContext = referenceContext ?? throw new ArgumentNullException(nameof(referenceContext));
        _referenceContainer = referenceContainer ?? throw new ArgumentNullException(nameof(referenceContainer));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task InitializeReferencesAsync()
    {
        var errors = await _identityContext.Errors.ToListAsync();
        _referenceContainer.Errors = _mapper.Map<List<ErrorViewModel>>(errors);

        var languages = await _referenceContext.Languages.ToListAsync();
        _referenceContainer.Languages = _mapper.Map<List<LanguageViewModel>>(languages);
    }
}