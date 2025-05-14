using Bogus;
using Tomou.Communication.Requests.User.Login;

namespace Tomou.TestUtils.Users.Login.Requests;
public static class RequestLoginUserJsonBuilder
{
    public static RequestLoginUserJson Build()
    {
        return new Faker<RequestLoginUserJson>()
            .RuleFor(l => l.Email, faker => faker.Internet.Email())
            .RuleFor(l => l.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
