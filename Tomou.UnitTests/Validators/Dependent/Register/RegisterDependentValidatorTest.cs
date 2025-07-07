using Shouldly;
using Tomou.Application.UseCases.Dependent.Register;
using Tomou.Exception;
using Tomou.TestUtils.Dependent.Request;

namespace Tomou.UnitTests.Validators.Dependent.Register;
public class RegisterDependentValidatorTest
{
    [Fact]

    public void Success()
    {
        var validator = new RegisterDependentValidator();
        var request = RequestRegisterDependentJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("         ")]
    public void Error_Name_Empty(string name)
    {
        var validator = new RegisterDependentValidator();
        var request = RequestRegisterDependentJsonBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);
        var error = result.Errors.ShouldHaveSingleItem();

        error.ShouldSatisfyAllConditions(e => e.ErrorMessage.ShouldBe(ResourceErrorMessages.EMPTY_NAME));
        result.IsValid.ShouldBeFalse();
    }
}
