using AutoMapper;
using Tomou.Application.Services.Auth;
using Tomou.Communication.Responses.Medications.Get;
using Tomou.Domain.Repositories.Medications;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;
using Tomou.Exception;
using Tomou.Domain.Repositories.Dependent;

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
    public async Task<ResponseMedicationsJson> Execute(long? userOrDependentId, string? nameFilter = null, bool ascending = true)
    {
        
        var userId = _userContext.GetUserId();
        var user = await _userRepository.GetUserById(userId) ?? throw new ForbiddenAccessException(ResourceErrorMessages.UNAUTHORIZED);
        long finalId;

        if (user.IsCaregiver)
        {
            if(userOrDependentId is null)
                throw new NotFoundException(ResourceErrorMessages.DEPENDENT_NOT_FOUND);

            var dependent = await _dependentRepository.GetByIdAsync(userOrDependentId.Value);
            if (dependent == null)
                throw new ForbiddenAccessException(ResourceErrorMessages.UNAUTHORIZED);

            finalId = userOrDependentId.Value;
        }

        else
        {
            finalId = userId;
        }

        var result = await _repository.GetMedications(finalId, user.IsCaregiver, nameFilter, ascending);

        return new ResponseMedicationsJson
        {
            Medications = _mapper.Map<List<ResponseMedicationShortJson>>(result)
        };
    }
}
