using System.ComponentModel.DataAnnotations;
using Tomou.Communication.Requests.User;
using Tomou.Communication.Responses.User;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Application.UseCases.User.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserReadOnlyRepository _readRepository;
    private readonly IUserWriteOnlyRepository _writeRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RegisterUserUseCase(IUserReadOnlyRepository readRepository, IUserWriteOnlyRepository writeRepository, IUnitOfWork unitOfWork)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _unitOfWork = unitOfWork;

    }
    public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        Validator(request);
        var entity = new RequestRegisterUserJson
        {
            Email = request.Email,
            Name = request.Name,
            Password = request.Password,
        };
        var result = _writeRepository.Add(entity);
        return Task.FromResult(new ResponseRegisteredUserJson
        {
            Name = request.Name,
        });
    }

    private void Validator(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
