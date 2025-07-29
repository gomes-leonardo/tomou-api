namespace Tomou.TestUtils.Dependent.Update.Request;
using Bogus;
using Tomou.Communication.Requests.Dependent.Register;

public static class RequestUpdateDependentJsonBuilder
{
    public static RequestUpdateDependentJson Build()
    {
        return new Faker<RequestUpdateDependentJson>()
            .RuleFor(dependent => dependent.Name, faker => faker.Person.FirstName);
    }
}
