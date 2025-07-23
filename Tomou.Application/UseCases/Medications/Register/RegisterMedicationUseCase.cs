using AutoMapper;
using System.Diagnostics;
using Tomou.Application.Services.Auth;
using Tomou.Communication.Requests.Medications.Register;
using Tomou.Communication.Responses.Medications.Register;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.Medications;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Application.UseCases.Medications.Register;
public class RegisterMedicationUseCase : IRegisterMedicationUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMedicationsWriteOnlyRepository _medicationRepository;
    private readonly IDependentReadOnlyRepository _dependent;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserContext _userContext;
    public RegisterMedicationUseCase(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IMedicationsWriteOnlyRepository medicationRepository,
        IDependentReadOnlyRepository dependent,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserContext userContext)
    {
        _medicationRepository = medicationRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userContext = userContext;
        _dependent = dependent;
    }
    public async Task<ResponseRegisterMedicationJson> Execute(RequestRegisterMedicationsJson request)
    {
        Validator(request);

        var userId = _userContext.GetUserId();
        var user = await _userReadOnlyRepository.GetUserById(userId);
         
        if (user is null)
        {
            throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);
        }

        var medication = _mapper.Map<Medication>(request);

        if (user.IsCaregiver)
        {
            if (request.DependentId is null)
                throw new NotFoundException(ResourceErrorMessages.DEPENDENT_NOT_FOUND);
           
            var dependent = await _dependent.GetByIdAsync((Guid)request.DependentId);
            if (dependent is null)
                throw new NotFoundException(ResourceErrorMessages.INVALID_DEPENDENT_CURRENT_CAREGIVER);

            if (dependent.CaregiverId != userId)
                throw new ForbiddenAccessException(ResourceErrorMessages.FORBIDDEN_ACCESS);

            medication.DependentId = dependent.Id;
        }

        else
        {
            if(request.DependentId is not null)
            {
                throw new ForbiddenAccessException(ResourceErrorMessages.FORBIDDEN_ACCESS);
            }

            medication.UserId = userId;
        }

        await _medicationRepository.Add(medication);
        await _unitOfWork.Commit();

        var response = _mapper.Map<ResponseRegisterMedicationJson>(medication);
        response.Message = $"Medicamento {medication.Name} cadastrado(a) com sucesso";
        return response;
    }

    private void Validator(RequestRegisterMedicationsJson request)
    {
        var validator = new RegisterMedicationValidator();
        var result = validator.Validate(request);

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
