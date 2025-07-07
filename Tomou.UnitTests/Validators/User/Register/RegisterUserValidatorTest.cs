using Shouldly;
using Tomou.Application.UseCases.User.Register;
using Tomou.Exception;
using Tomou.TestUtils.User.Register.Requests;

namespace Tomou.UnitTests.Validators.User.Register;
public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("         ")]
    public void Error_Email_Empty(string email)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);
        var error = result.Errors.ShouldHaveSingleItem();

        error.ShouldSatisfyAllConditions(e => e.ErrorMessage.ShouldBe(ResourceErrorMessages.EMPTY_EMAIL));
        result.IsValid.ShouldBeFalse();
    }


    [Theory]
    [InlineData("email.com")]
    [InlineData("email@")]
    [InlineData("@email.com")]
    public void Error_Email_Invalid(string email)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);
        var error = result.Errors.ShouldHaveSingleItem();

        error.ShouldSatisfyAllConditions(e => e.ErrorMessage.ShouldBe(ResourceErrorMessages.INVALID_EMAIL));
        result.IsValid.ShouldBeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("         ")]
    public void Error_Name_Empty(string name)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);
        var error = result.Errors.ShouldHaveSingleItem();

        error.ShouldSatisfyAllConditions(e => e.ErrorMessage.ShouldBe(ResourceErrorMessages.EMPTY_NAME));
    }

    [Theory]
    [InlineData("")]
    [InlineData("         ")]
    public void Error_Password_Empty(string password)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = password;

        var result = validator.Validate(request);
        var error = result.Errors.ShouldHaveSingleItem();

        error.ShouldSatisfyAllConditions(e => e.ErrorMessage.ShouldBe(ResourceErrorMessages.EMPTY_PASSWORD));
    }

    [Theory]
    [InlineData("Password123")]
    [InlineData("password!")]
    [InlineData("Pass1234")]

    public void Error_Password_Invalid(string password)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = password;

        var result = validator.Validate(request);
        var error = result.Errors.ShouldHaveSingleItem();

        error.ShouldSatisfyAllConditions(e => e.ErrorMessage.ShouldBe(ResourceErrorMessages.INVALID_PASSWORD));
    }

    [Theory]
    [InlineData(false)]
    public void IsCaregiver_False(bool isCaregiver)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.IsCaregiver = isCaregiver;

        var result = validator.Validate(request);
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(true)]
    public void IsCaregiver_True(bool isCaregiver)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.IsCaregiver = isCaregiver;

        var result = validator.Validate(request);
        result.IsValid.ShouldBeTrue();
    }

}
