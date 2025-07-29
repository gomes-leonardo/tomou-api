using Tomou.Communication.Requests.Dependent.Register;
using Bogus;

namespace Tomou.TestUtils.Dependent.Register.Request;
public static class RequestRegisterDependentJsonBuilder
{
    public static RequestRegisterDependentJson Build()
    {
        return new Faker<RequestRegisterDependentJson>()
            .RuleFor(dependent => dependent.Name, faker => faker.Person.FirstName);
    }
}
