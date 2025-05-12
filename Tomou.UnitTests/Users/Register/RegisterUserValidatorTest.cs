using Shouldly;
using Tomou.Application.UseCases.User.Register;
using Tomou.TestUtils.Users.Register.Requests;

namespace Tomou.UnitTests.Users.Register;
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
}
