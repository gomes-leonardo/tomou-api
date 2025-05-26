using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace Tomou.IntegrationTests.Controllers;
public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    public UserControllerTests(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task ShouldRegisterUser_WhenDataIsValid()
    {
        var faker = new Faker("pt_BR");
        var name = faker.Name.FullName();
        var email = faker.Internet.Email(name);
        var request = new
        {
            name,
            email,
            password = "!Senha123",
            isCaregiver = true
        };

        var content = new StringContent
        (
            System.Text.Json.JsonSerializer.Serialize(request),
            System.Text.Encoding.UTF8,
            "application/json"
        );


        var response = await _httpClient.PostAsync("/api/user/register", content);
        var json = await response.Content.ReadAsStringAsync();
        json.ShouldContain(name);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenEmailAlreadyExists()
    {
        var faker = new Faker("pt_BR");
        var name = faker.Name.FullName();
        var email = faker.Internet.Email(name);
        var request = new
        {
            name,
            email,
            password = "!Senha123",
            isCaregiver = true
        };

        var content = new StringContent
        (
            System.Text.Json.JsonSerializer.Serialize(request),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var firstResponse = await _httpClient.PostAsync("/api/user/register", content);
        firstResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

        var duplicatedContent = new StringContent
        (
            System.Text.Json.JsonSerializer.Serialize(request),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var secondResponse = await _httpClient.PostAsync("/api/user/register", duplicatedContent);
        secondResponse.StatusCode.ShouldBe(HttpStatusCode.Conflict);


    }
}
