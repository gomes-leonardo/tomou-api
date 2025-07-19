using AutoMapper;
using Tomou.Application.Services.Auth;
using Tomou.Communication.Requests.Medications.Update;
using Tomou.Communication.Responses.Medications.Update;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.Medications;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Application.UseCases.Medications.Update;
public class UpdateMedicationUseCase : IUpdateMedicationUseCase
{
    private readonly IMedicationsWriteOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IUserReadOnlyRepository _userRepository;
    private readonly IMedicationsReadOnlyRepository _medicationReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMedicationUseCase(
        IMedicationsWriteOnlyRepository repository,
        IMedicationsReadOnlyRepository medicationReadOnlyRepository,
        IMapper mapper,
        IUserContext userContext,
        IUserReadOnlyRepository userRepository,
        IUnitOfWork unitOfWork)
        
    {
        _repository = repository;
        _mapper = mapper;
        _userContext = userContext;
        _userRepository = userRepository;
        _medicationReadOnlyRepository = medicationReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseUpdatedMedicationJson> Execute(Guid? id, Guid medicamentId, RequestUpdateMedicationJson request)
    {
       var userId = _userContext.GetUserId();
       var user = await _userRepository.GetUserById(userId) ?? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);
       Guid ownerId;

        if (user.IsCaregiver)
        {
            if(id is null)
                throw new NotFoundException(ResourceErrorMessages.DEPENDENT_NOT_FOUND);

            ownerId = id.Value;
        }

        else
        {
            ownerId = userId;
        }
        var medication = await _medicationReadOnlyRepository
       .GetMedicationsById(ownerId, user.IsCaregiver, medicamentId)
       ?? throw new NotFoundException(ResourceErrorMessages.MEDICATION_NOT_FOUND);
       
        _repository.Update(medication);
        _mapper.Map(request, medication);
        await _unitOfWork.Commit();

        var response = _mapper.Map<ResponseUpdatedMedicationJson>(medication);
        response.Message = $"Medicamento {medication.Name} atualizado com sucesso";
        return response;
    }
}
