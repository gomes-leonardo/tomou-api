using AutoMapper;
using Tomou.Application.Services.Auth;
using Tomou.Communication.Responses.Medications.Get;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.Medications;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;
using Tomou.Exception;

namespace Tomou.Application.UseCases.Medications.GetById;
public class GetMedicationByIdUseCase : IGetMedicationByIdUseCase
{
    private readonly IMedicationsReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUserReadOnlyRepository _userRepository;
    private readonly IDependentReadOnlyRepository _dependentRepository;
    private readonly IUserContext _userContext;
    public GetMedicationByIdUseCase(
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
    public async Task<ResponseMedicationShortJson> Execute(Guid? id, Guid medicamentId)
    {
        var userId = _userContext.GetUserId();
        var user = await _userRepository.GetUserById(userId) ?? throw new ForbiddenAccessException(ResourceErrorMessages.UNAUTHORIZED);
        Guid ownerId;

        if (user.IsCaregiver)
        {
            if (id is null)
                throw new NotFoundException(ResourceErrorMessages.DEPENDENT_NOT_FOUND);

            var dependent = await _dependentRepository.GetByIdAsync(id.Value);
            if (dependent == null)
                throw new ForbiddenAccessException(ResourceErrorMessages.UNAUTHORIZED);

            ownerId = id.Value;
        }

        else
        {
            ownerId = userId;
        }

        var medication = await _repository.GetMedicationsById(ownerId, user.IsCaregiver, medicamentId) ?? throw new NotFoundException(ResourceErrorMessages.MEDICATION_NOT_FOUND);

        return _mapper.Map<ResponseMedicationShortJson>(medication);
    }

   
}
