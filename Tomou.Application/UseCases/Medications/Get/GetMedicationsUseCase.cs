using AutoMapper;
using Tomou.Application.Services.Auth;
using Tomou.Communication.Responses.Medications.Get;
using Tomou.Domain.Repositories.Medications;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;
using Tomou.Exception;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.Medications.Filters;

namespace Tomou.Application.UseCases.Medications.Get;
public class GetMedicationsUseCase : IGetMedicationsUseCase
{
    private readonly IMedicationsReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUserReadOnlyRepository _userRepository;
    private readonly IDependentReadOnlyRepository _dependentRepository;
    private readonly IUserContext _userContext;

    public GetMedicationsUseCase(
        IMedicationsReadOnlyRepository repository,
        IMapper mapper,
        IUserContext userContext,
        IUserReadOnlyRepository userRepository,
        IDependentReadOnlyRepository dependentRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _userContext = userContext;
        _userRepository = userRepository;
        _dependentRepository = dependentRepository;
    }
    public async Task<ResponseMedicationsJson> Execute(Guid? id, string? nameFilter = null, bool ascending = true)
    {
        
        var userId = _userContext.GetUserId();
        var user = await _userRepository.GetUserById(userId) ?? throw new ForbiddenAccessException(ResourceErrorMessages.UNAUTHORIZED);
        Guid ownerId;

        if (user.IsCaregiver)
        {
            if(id is null)
                throw new NotFoundException(ResourceErrorMessages.DEPENDENT_NOT_FOUND);

            var dependent = await _dependentRepository.GetByIdAsync(id.Value);
            if (dependent == null)
                throw new ForbiddenAccessException(ResourceErrorMessages.UNAUTHORIZED);

            var isOwner = dependent.CaregiverId == userId;
            if(!isOwner)
                throw new ForbiddenAccessException(ResourceErrorMessages.INVALID_DEPENDENT_CURRENT_CAREGIVER);

            ownerId = id.Value;
        }

        else
        {
            ownerId = userId;
        }

        var filter = new MedicationsFilter(
            ownerId:    ownerId,
            isCaregiver: user.IsCaregiver,
            nameContains: nameFilter,
            ascending:  ascending
        );

        var result = await _repository.GetMedicationsByOwner(filter);

        return new ResponseMedicationsJson
        {
            Medications = _mapper.Map<List<ResponseMedicationShortJson>>(result)
        };
    }
}
