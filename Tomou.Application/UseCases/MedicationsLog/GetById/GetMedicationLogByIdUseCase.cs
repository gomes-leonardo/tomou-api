using AutoMapper;
using Tomou.Application.Services.Auth;
using Tomou.Communication.Responses.MedicationLog.Get;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.MedicatioLog;
using Tomou.Domain.Repositories.User;
using Tomou.Exception;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Application.UseCases.MedicationsLog.GetById;
public class GetMedicationLogByIdUseCase : IGetMedicationLogByIdUseCase
{
    private readonly IMedicationsLogReadOnlyRepository _medicationsLogRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IUserReadOnlyRepository _userRepository;
    private readonly IDependentReadOnlyRepository _dependentRepository;


    public GetMedicationLogByIdUseCase(
        IMedicationsLogReadOnlyRepository medicationsLogRepository,
        IMapper mapper,
        IUserContext userContext,
        IUserReadOnlyRepository userRepository,
        IDependentReadOnlyRepository dependentRepository)
    {
        _medicationsLogRepository = medicationsLogRepository;
        _mapper = mapper;
        _userContext = userContext;
        _userRepository = userRepository;
        _dependentRepository = dependentRepository;
    }
    public async Task<ResponseMedicationLogShortJson> Execute(Guid id, Guid medicationLogId)
    {
        var userId = _userContext.GetUserId();
        var user = await _userRepository.GetUserById(userId) ?? throw new ForbiddenAccessException(ResourceErrorMessages.UNAUTHORIZED);
        Guid ownerId;

        if (user.IsCaregiver)
        {
            var dependent = await _dependentRepository.GetByIdAsync(id) ?? throw new NotFoundException(ResourceErrorMessages.DEPENDENT_NOT_FOUND);

            var isOwner = dependent.CaregiverId == id;

            ownerId = dependent.CaregiverId;
        }

        else
        {
            ownerId = userId;
        }

        var result = await _medicationsLogRepository.GetMedicationLogById(ownerId, user.IsCaregiver, medicationLogId);

        return _mapper.Map<ResponseMedicationLogShortJson>(result);
    }
}
