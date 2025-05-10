using System.ComponentModel.DataAnnotations;
using Tomou.Communication.Requests.User;
using Tomou.Communication.Responses.User;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Application.UseCases.User.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        Validator(request);
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
