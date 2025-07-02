using AutoMapper;
using Tomou.Application.Services.Auth;
using Tomou.Communication.Requests.Dependent.Register;
using Tomou.Communication.Responses.Dependent.Register;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Application.UseCases.Dependent.Register;
public class RegisterDependentUseCase : IRegisterDependentUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDependentWriteOnlyRepository _depentWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserContext _userContext;

    public RegisterDependentUseCase(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IDependentWriteOnlyRepository dependentWriteOnlyRepository,
        IUserReadOnlyRepository readOnlyRepository,
        IUserContext userContext
        )
    {
        _depentWriteOnlyRepository = dependentWriteOnlyRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userReadOnlyRepository = readOnlyRepository;
        _userContext = userContext;
    }
    public async Task<ResponseCreateDependentJson> Execute(RequestRegisterDependentJson request)
    {
        Validator(request);
        var caregiverId = _userContext.GetUserId();

        var user = await _userReadOnlyRepository.GetUserById(caregiverId);
        if (user is null || user.IsCaregiver is false)
            throw new ForbiddenAccessException(ResourceErrorMessages.FORBIDDEN_ACCESS);

        if (user.Dependents.Count >= 5)
            throw new LimitExceededException(ResourceErrorMessages.LIMIT_EXCEED);

        var entity = _mapper.Map<Domain.Entities.Dependent>(request);
        entity.CaregiverId = caregiverId;


        await _depentWriteOnlyRepository.Add(entity);
        await _unitOfWork.Commit();

       var response = _mapper.Map<ResponseCreateDependentJson>(entity);
       response.Message = $"Dependente {entity.Name} cadastrado(a) com sucesso";

       return response;

    }

    private void Validator(RequestRegisterDependentJson request)
    {
        var validator = new RegisterDependentValidator();
        var result = validator.Validate(request);

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
