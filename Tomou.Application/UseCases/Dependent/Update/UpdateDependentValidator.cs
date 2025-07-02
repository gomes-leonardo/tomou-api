using FluentValidation;
using Tomou.Application.UseCases.Dependent.Validators.Common;
using Tomou.Communication.Requests.Dependent.Register;

namespace Tomou.Application.UseCases.Dependent.Update;
public class UpdateDependentValidator : AbstractValidator<RequestUpdateDependentJson>
{
    public UpdateDependentValidator()
    {
        Include(new NameValidator<RequestUpdateDependentJson>(x => x.Name));
    }
}
