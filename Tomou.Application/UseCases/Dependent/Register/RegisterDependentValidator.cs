using FluentValidation;
using Tomou.Application.UseCases.Dependent.Validators.Common;
using Tomou.Communication.Requests.Dependent.Register;
using Tomou.Exception;

namespace Tomou.Application.UseCases.Dependent.Register;
public class RegisterDependentValidator : AbstractValidator<RequestRegisterDependentJson>
{
    public RegisterDependentValidator()
    {
        Include(new NameValidator<RequestRegisterDependentJson>(x => x.Name));
    }
}
