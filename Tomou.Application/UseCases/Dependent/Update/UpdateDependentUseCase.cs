using AutoMapper;
using Tomou.Application.Services.Auth;
using Tomou.Communication.Requests.Dependent.Register;
using Tomou.Communication.Responses.Dependent.Update;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Application.UseCases.Dependent.Update;
public class UpdateDependentUseCase : IUpdateDependentUseCase
{
    private readonly IDependentUpdateOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserContext _userContext;

    public UpdateDependentUseCase(
        IDependentUpdateOnlyRepository repository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserContext userContext)
    {
        _mapper = mapper;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userContext = userContext;

    }
    public async Task<ResponseUpdatedDependentJson> Execute(RequestUpdateDependentJson request, long id)
    {
        Validator(request);
        var caregiverId = _userContext.GetUserId();
        var user = await _userReadOnlyRepository.GetUserById(caregiverId);

        if(user is null || user.IsCaregiver is false)
            throw new ForbiddenAccessException(ResourceErrorMessages.FORBIDDEN_ACCESS);

        var dependent = await _repository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessages.DEPENDENT_NOT_FOUND);

        _repository.Update(dependent);
        _mapper.Map(request, dependent);

        await _unitOfWork.Commit();

        var response = _mapper.Map<ResponseUpdatedDependentJson>(dependent);
        response.Message = $"Dependente atualizado(a) com sucesso";

        return response;
    }


    private void Validator(RequestUpdateDependentJson request)
    {
        var validator = new UpdateDependentValidator();
        var result = validator.Validate(request);

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
