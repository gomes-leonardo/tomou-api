using AutoMapper;
using Tomou.Communication.Requests.Dependent.Register;
using Tomou.Communication.Responses.Dependent.Register;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Application.UseCases.Dependent.Register;
public class RegisterDependentUseCase : IRegisterDependentUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDependentWriteOnlyRepository _depentWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;

    public RegisterDependentUseCase(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IDependentWriteOnlyRepository dependentWriteOnlyRepository,
        IUserReadOnlyRepository readOnlyRepository)
    {
        _depentWriteOnlyRepository = dependentWriteOnlyRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userReadOnlyRepository = readOnlyRepository;
    }
    public async Task<ResponseCreateDependentJson> Execute(long caregiverId, RequestRegisterDependentJson request)
    {
        Validator(request);
        var entity = _mapper.Map<Domain.Entities.Dependent>(request);
        entity.CaregiverId = caregiverId;

        var user = await _userReadOnlyRepository.GetUserById(caregiverId);
        if (user is null || user.IsCaregiver is false)
            throw new System.Exception("Only caregivers can register dependents.");

        await _depentWriteOnlyRepository.Add(entity);
        await _unitOfWork.Commit();

       return _mapper.Map<ResponseCreateDependentJson>(entity);

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
