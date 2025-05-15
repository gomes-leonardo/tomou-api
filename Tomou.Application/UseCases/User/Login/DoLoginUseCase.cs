using System.ComponentModel.DataAnnotations;
using Tomou.Communication.Requests.User.Login;
using Tomou.Communication.Responses.User.Login;
using Tomou.Domain.Repositories.User;
using Tomou.Domain.Security.Crypthography;
using Tomou.Domain.Security.Tokens;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Application.UseCases.User.Login;
public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly IEncrypter _encrypter;
    private readonly IAccessTokenGenerator _tokenGenerator;
    public DoLoginUseCase(IUserReadOnlyRepository repository, IEncrypter encrypter, IAccessTokenGenerator tokenGenerator)
    {
        _repository = repository;
        _encrypter = encrypter;
        _tokenGenerator = tokenGenerator;
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

        var token = _tokenGenerator.Generate(user.Id, user.Name, user.Email, user.IsCaregiver);

        return new ResponseLoggedUserJson
        {
            Name = user.Name,
            Token = token
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
