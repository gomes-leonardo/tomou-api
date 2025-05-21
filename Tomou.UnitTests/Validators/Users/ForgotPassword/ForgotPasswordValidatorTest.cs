using IdentityModel.OidcClient;
using Shouldly;
using Tomou.Application.UseCases.User.Register;
using Tomou.Application.Validators.User;
using Tomou.Exception;
using Tomou.TestUtils.Users.Login.Requests;
using Tomou.TestUtils.Users.Register.Requests;

namespace Tomou.UnitTests.Validators.Users.ForgotPassword;
public class ForgotPasswordValidatorTest
{

    [Fact]
    public void Success()
    {
        var validator = new ForgotPasswordValidator();
        var request = RequestForgotPasswordJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }


    [Theory]
    [InlineData("email.com")]
    [InlineData("email@")]
    [InlineData("@email.com")]
    public void Error_Email_Invalid(string email)
    {
        var validator = new ForgotPasswordValidator();
        var request = RequestForgotPasswordJsonBuilder.Build();

        request.Email = email;

        var result = validator.Validate(request);
        var error = result.Errors.ShouldHaveSingleItem();


        error.ShouldSatisfyAllConditions(e => e.ErrorMessage.ShouldBe(ResourceErrorMessages.INVALID_EMAIL));
        result.IsValid.ShouldBeFalse();

    }
}
