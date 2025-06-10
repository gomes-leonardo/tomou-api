using Tomou.Communication.Requests.User.ForgotPassword;
using Bogus;

namespace Tomou.TestUtils.Users.ResetPassword.Requests;
public static class RequestResetPasswordJsonBuilder
{
   public static RequestResetPasswordJson Build()
    {
        return new Faker<RequestResetPasswordJson>()
            .RuleFor(u => u.Token, faker => faker.Random.Replace("#####"))
            .RuleFor(u => u.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
