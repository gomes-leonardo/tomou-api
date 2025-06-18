using FluentValidation;
using Tomou.Communication.Requests.Dependent.Register;
using Tomou.Exception;

namespace Tomou.Application.UseCases.Dependent.Register;
public class RegisterDependentValidator : AbstractValidator<RequestRegisterDependentJson>
{
    public RegisterDependentValidator()
    {
        RuleFor(u => u.Name).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_NAME);
    }
}
