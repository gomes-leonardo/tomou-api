using Shouldly;
using Tomou.Application.UseCases.User.Login;
using Tomou.Application.UseCases.User.Register;
using Tomou.Communication.Requests.User.Login;
using Tomou.Exception;
using Tomou.TestUtils.User.Login.Requests;
using Tomou.TestUtils.User.Register.Requests;

namespace Tomou.UnitTests.Validators.User.Login;
public class LoginUserValidatorTest
{
    [Fact]
    public void Sucess()
    {
       var validator = new LoginValidator();
       var request = RequestLoginUserJsonBuilder.Build();
       var result = validator.Validate(request);

       result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("            ")]
    [InlineData("")]
    public void Error_Email_Empty(string email)
    {
        var validator = new LoginValidator();
        var request = RequestLoginUserJsonBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

    }

    [Theory]
    [InlineData("email.com")]
    [InlineData("email@")]
    [InlineData("@email.com")]
    public void Error_Email_Invalid(string email)
    {
        var validator = new LoginValidator();
        var request = RequestLoginUserJsonBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);
        var error = result.Errors.ShouldHaveSingleItem();

        error.ShouldSatisfyAllConditions(e => e.ErrorMessage.ShouldBe(ResourceErrorMessages.INVALID_EMAIL));
        result.IsValid.ShouldBeFalse();
    }

    [Theory]
    [InlineData("            ")]
    [InlineData("")]
    public void Error_Password_Empty(string password)
    {
        var validator = new LoginValidator();
        var request = RequestLoginUserJsonBuilder.Build();
        request.Password = password;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

    }
}
