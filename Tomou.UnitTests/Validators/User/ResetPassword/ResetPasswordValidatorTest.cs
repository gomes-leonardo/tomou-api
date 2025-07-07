using Shouldly;
using Tomou.Application.UseCases.User.Login;
using Tomou.Application.UseCases.User.ResetPassword;
using Tomou.TestUtils.User.Login.Requests;
using Tomou.TestUtils.User.ResetPassword.Requests;

namespace Tomou.UnitTests.Validators.User.ResetPassword;
public class ResetPasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new ResetPasswordValidator();
        var request = RequestResetPasswordJsonBuilder.Build();

        var result = validator.Validate(request);
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("            ")]
    [InlineData("")]
    public void Error_Token_Empty(string token)
    {
        var validator = new ResetPasswordValidator();
        var request = RequestResetPasswordJsonBuilder.Build();

        request.Token = token;
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
    }
    
    [Theory]
    [InlineData("1234")]
    [InlineData("123")]
    [InlineData("12")]
    [InlineData("1")]
    public void Error_Token_Less_Than_Five(string token)
    {
        var validator = new ResetPasswordValidator();
        var request = RequestResetPasswordJsonBuilder.Build();

        request.Token = token;
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
    }

    [Theory]
    [InlineData("            ")]
    [InlineData("")]
    public void Error_Password_Empty(string password)
    {
        var validator = new ResetPasswordValidator();
        var request = RequestResetPasswordJsonBuilder.Build();
        request.NewPassword = password;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

    }

    [Theory]
    [InlineData("abc123")]
    [InlineData("Abc123!")]
    [InlineData("abc!")]
    [InlineData("Abc!")]
    public void Error_Password_Invalid_Format(string password)
    {
        var validator = new ResetPasswordValidator();
        var request = RequestResetPasswordJsonBuilder.Build();
        request.NewPassword = password;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

    }
}
