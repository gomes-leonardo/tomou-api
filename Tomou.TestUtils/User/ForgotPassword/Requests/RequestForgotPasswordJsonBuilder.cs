using Bogus;
using Tomou.Communication.Requests.User.ForgotPassword;
using Tomou.Communication.Requests.User.Login;

namespace Tomou.TestUtils.User.Login.Requests;
public static class RequestForgotPasswordJsonBuilder
{
    public static RequestForgotPasswordJson Build()
    {
        return new Faker<RequestForgotPasswordJson>()
            .RuleFor(l => l.Email, faker => faker.Internet.Email());
    }
}
