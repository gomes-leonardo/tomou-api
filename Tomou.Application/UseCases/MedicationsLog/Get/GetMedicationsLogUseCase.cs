using AutoMapper;
using Tomou.Application.Services.Auth;
using Tomou.Communication.Responses.MedicationLog.Get;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.MedicatioLog;
using Tomou.Domain.Repositories.User;
using Tomou.Exception;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Application.UseCases.MedicationsLog.Get;
public class GetMedicationsLogUseCase : IGetMedicationsLogUseCase
{
    private readonly IMedicationsLogReadOnlyRepository _medicationsLogRepository;
    private readonly IMapper _mapper;
    private readonly IUserReadOnlyRepository _userRepository;
    private readonly IDependentReadOnlyRepository _dependentRepository;
    private readonly IUserContext _userContext;


    public GetMedicationsLogUseCase(
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
    public async Task<ResponseMedicationsLogJson> Execute(MedicationLogFilter filter)
    {
        var userId = _userContext.GetUserId();
        var user = await _userRepository.GetUserById(userId) ?? throw new ForbiddenAccessException(ResourceErrorMessages.UNAUTHORIZED);


        if(user.IsCaregiver)
        {
            var dependent = await _dependentRepository.GetByIdAsync(filter.OwnerId) ?? throw new NotFoundException(ResourceErrorMessages.DEPENDENT_NOT_FOUND);

            var isOwner = dependent.CaregiverId == userId;
            if(!isOwner)
                throw new ForbiddenAccessException(ResourceErrorMessages.INVALID_DEPENDENT_CURRENT_CAREGIVER);
        }
        else
        {
            if (filter.OwnerId != userId)
                throw new ForbiddenAccessException(ResourceErrorMessages.UNAUTHORIZED);
        }

        var result = await _medicationsLogRepository.GetMedicationLog(filter);

        return new ResponseMedicationsLogJson
        {
            MedicationsLog = _mapper.Map<List<ResponseMedicationLogShortJson>>(result)
        };

    }
}
