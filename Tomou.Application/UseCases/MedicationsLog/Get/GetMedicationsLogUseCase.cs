using AutoMapper;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.MedicationsLog.Get.Factories;
using Tomou.Application.UseCases.MedicationsLog.Get.Validators;
using Tomou.Communication.Requests.MedicationsLog.MedicationLogQuery;
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
    private readonly IMedicationLogFilterFactory _filterFactory;


    public GetMedicationsLogUseCase(
        IMedicationsLogReadOnlyRepository medicationsLogRepository,
        IMapper mapper,
        IUserContext userContext,
        IUserReadOnlyRepository userRepository,
        IDependentReadOnlyRepository dependentRepository,
        IMedicationLogFilterFactory filterFactory)
    {
        _medicationsLogRepository = medicationsLogRepository;
        _mapper = mapper;
        _userContext = userContext;
        _userRepository = userRepository;
        _dependentRepository = dependentRepository;
        _filterFactory = filterFactory;
    }
    public async Task<ResponseMedicationsLogJson> Execute(MedicationLogQuery query)
    {

        Validator(query);

        var userId = _userContext.GetUserId();
        var user = await _userRepository.GetUserById(userId) ?? throw new ForbiddenAccessException(ResourceErrorMessages.UNAUTHORIZED);
        Guid ownerId;

        if(user.IsCaregiver)
        {
            var dependent = await _dependentRepository.GetByIdAsync(query.Id.Value) ?? throw new NotFoundException(ResourceErrorMessages.DEPENDENT_NOT_FOUND);

            var isOwner = dependent.CaregiverId == userId;
            if(!isOwner)
                throw new ForbiddenAccessException(ResourceErrorMessages.INVALID_DEPENDENT_CURRENT_CAREGIVER);

            ownerId = dependent.CaregiverId;
        }
        else
        {
            ownerId = userId;
        }

        var filter = _filterFactory.Create(query, ownerId, user.IsCaregiver);

        var result = await _medicationsLogRepository.GetMedicationLog(filter);

        return new ResponseMedicationsLogJson
        {
            MedicationsLog = _mapper.Map<List<ResponseMedicationLogShortJson>>(result)
        };

    }

    private void Validator(MedicationLogQuery query)
    {
        var validator = new MedicationLogQueryValidator();
        var result = validator.Validate(query);

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
