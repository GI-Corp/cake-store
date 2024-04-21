using AutoMapper;
using CakeStore.Infrastructure.DAL.DbContexts;
using CakeStoreApp.Mappers.Reference;
using Microsoft.EntityFrameworkCore;
using Shared.Presentation.ViewModels.Reference;

namespace CakeStoreApp.Helpers;

public class ReferenceContainerInitializer
{
    private readonly CakeStoreContext _cakeStoreContext;
    private readonly IReferenceContainer _referenceContainer;
    private readonly ReferenceContext _referenceContext;
    private readonly IMapper _mapper;

    public ReferenceContainerInitializer(CakeStoreContext storeContext, ReferenceContext referenceContext, 
        IMapper mapper, IReferenceContainer referenceContainer)
    {
        _cakeStoreContext = storeContext ?? throw new ArgumentNullException(nameof(storeContext));
        _referenceContext = referenceContext ?? throw new ArgumentNullException(nameof(referenceContext));
        _referenceContainer = referenceContainer ?? throw new ArgumentNullException(nameof(referenceContainer));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task InitializeReferencesAsync()
        {
            var errors = await _cakeStoreContext.Errors.ToListAsync();
            _referenceContainer.Errors = _mapper.Map<List<ErrorViewModel>>(errors);

            var languages = await _referenceContext.Languages.ToListAsync();
            _referenceContainer.Languages = _mapper.Map<List<LanguageViewModel>>(languages);
        }
}