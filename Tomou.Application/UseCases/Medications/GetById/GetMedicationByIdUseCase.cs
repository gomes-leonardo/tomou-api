﻿using AutoMapper;
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
        var user = await _userRepository.GetUserById(userId) ?? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);
        Guid ownerId;

        if (user.IsCaregiver)
        {
            if (id is null)
                throw new NotFoundException(ResourceErrorMessages.DEPENDENT_NOT_FOUND);

            ownerId = id.Value;
        }

        else
        {
            ownerId = userId;
        }

        var medication = await _repository.GetMedicationsById(ownerId, user.IsCaregiver, medicamentId) ?? throw new NotFoundException(ResourceErrorMessages.MEDICATION_NOT_FOUND);

        var isMedicationOwner = medication.UserId == userId || medication.Dependent?.CaregiverId == userId;

        if (!isMedicationOwner)
            throw new ForbiddenAccessException(ResourceErrorMessages.INVALID_DEPENDENT_CURRENT_CAREGIVER);

        return _mapper.Map<ResponseMedicationShortJson>(medication);
    }

   
}
