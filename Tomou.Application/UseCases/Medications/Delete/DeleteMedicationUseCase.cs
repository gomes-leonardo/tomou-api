
using AutoMapper;
using Tomou.Application.Services.Auth;
using Tomou.Domain.Repositories.Medications;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;
using Tomou.Exception;
using Tomou.Domain.Repositories.Medications.Filters;

namespace Tomou.Application.UseCases.Medications.Delete;
public class DeleteMedicationUseCase : IDeleteMedicationUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IUserReadOnlyRepository _userRepository;
    private readonly IMedicationsWriteOnlyRepository _repository;
    private readonly IMedicationsReadOnlyRepository _medicationReadOnlyRepository;

    public DeleteMedicationUseCase(
        IMedicationsWriteOnlyRepository repository,
        IMedicationsReadOnlyRepository medicationReadOnlyRepository,
        IUserContext userContext,
        IUserReadOnlyRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _userContext = userContext;
        _userRepository = userRepository;
        _medicationReadOnlyRepository = medicationReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(Guid? id, Guid medicamentId)
    {
        var userId = _userContext.GetUserId();
        var user = await _userRepository.GetUserById(userId)
            ?? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

        Guid ownerId;
        if (user.IsCaregiver)
        {
            if (id is null)
                throw new NotFoundException(ResourceErrorMessages.DEPENDENT_NOT_FOUND);

            ownerId = id.Value;
        }
        else
        {
            if (id is not null)
                throw new ForbiddenAccessException(ResourceErrorMessages.UNAUTHORIZED);

            ownerId = userId;
        }

        var filter = new MedicationsFilterById(
            ownerId, user.IsCaregiver, medicamentId
        );

        var medication = await _medicationReadOnlyRepository
            .GetMedicationsById(filter)
            ?? throw new NotFoundException(ResourceErrorMessages.MEDICATION_NOT_FOUND);

        var isOwner = medication.UserId == userId
                   || medication.Dependent?.CaregiverId == userId;

        if (!isOwner)
            throw new ForbiddenAccessException(ResourceErrorMessages.INVALID_DEPENDENT_CURRENT_CAREGIVER);

        await _repository.Delete(medication.Id);
        await _unitOfWork.Commit();
    }

}
