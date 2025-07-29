using Shouldly;
using Tomou.Application.UseCases.Dependent.Update;
using Tomou.Exception;
using Tomou.TestUtils.Dependent.Update.Request;

namespace Tomou.UnitTests.Validators.Dependent.Update;
public class UpdateDependentValidatorTest
{
    [Fact]

    public void Success()
    {
        var validator = new UpdateDependentValidator();
        var request = RequestUpdateDependentJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("         ")]
    public void Error_Name_Empty(string name)
    {
        var validator = new UpdateDependentValidator();
        var request = RequestUpdateDependentJsonBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);
        var error = result.Errors.ShouldHaveSingleItem();

        error.ShouldSatisfyAllConditions(e => e.ErrorMessage.ShouldBe(ResourceErrorMessages.EMPTY_NAME));
        result.IsValid.ShouldBeFalse();
    }
}
