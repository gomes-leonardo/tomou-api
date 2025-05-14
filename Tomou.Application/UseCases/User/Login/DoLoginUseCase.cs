using System.ComponentModel.DataAnnotations;
using Tomou.Communication.Requests.User.Login;
using Tomou.Communication.Responses.User.Login;
using Tomou.Domain.Repositories.User;
using Tomou.Domain.Security;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Application.UseCases.User.Login;
public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly IEncrypter _encrypter;
    public DoLoginUseCase(IUserReadOnlyRepository repository, IEncrypter encrypter)
    {
        _repository = repository;
        _encrypter = encrypter;
    }
    public async Task<ResponseLoggedUserJson> Execute(RequestLoginUserJson request)
    {
        Validator(request);

        var user = await _repository.GetUserByEmail(request.Email) ?? throw new InvalidCredentialsException();
        var isPasswordValid = _encrypter.Compare(request.Password, user.Password);
        if(!isPasswordValid)
        {
            throw new InvalidCredentialsException();
        }

        return new ResponseLoggedUserJson
        {
            Name = user.Name,
        };

    }

    private void Validator(RequestLoginUserJson request)
    {
        var validator = new LoginValidator();
        var result = validator.Validate(request);

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
