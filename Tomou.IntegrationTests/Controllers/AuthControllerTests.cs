using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Text.Json;
using Tomou.Communication.Responses.User.Login;

namespace Tomou.IntegrationTests.Controllers;
public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    public AuthControllerTests(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task ShouldLoginUser_WhenDataIsValid()
    {
        var faker = new Faker("pt_BR");
        var name = faker.Name.FullName();
        var email = faker.Internet.Email(name);
        var registerRequest = new
        {
            name,
            email,
            password = "Teste@12345"
        };

        var registerContent = new StringContent
        (
            System.Text.Json.JsonSerializer.Serialize(registerRequest),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        await _httpClient.PostAsync("/api/user/", registerContent);

        var loginRequest = new
        {
           registerRequest.email,
           registerRequest.password
        };

        var loginContent = new StringContent
        (
            System.Text.Json.JsonSerializer.Serialize(loginRequest),
            System.Text.Encoding.UTF8,
            "application/json"
        );


        var loginResponse = await _httpClient.PostAsync("api/Auth", loginContent);

        loginResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

        var loginResponseBody = await loginResponse.Content.ReadAsStringAsync();

        var responseJson = JsonSerializer.Deserialize<ResponseLoggedUserJson>(loginResponseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        loginResponseBody.ShouldContain("token", Case.Sensitive);
    }

    [Fact]

    public async Task ShouldReturnUnauthorized_WhenEmailIsNotRegistered()
    {
        var faker = new Faker("pt_BR");
        var email = faker.Internet.Email();

        var request = new
        {
            email,
            password = "Mudar@1234"
        };

        var content = new StringContent
        (
            System.Text.Json.JsonSerializer.Serialize(request),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync("api/Auth", content);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);


    }
    [Fact]
    public async Task ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        var faker = new Faker("pt_BR");
        var name = faker.Name.FullName();
        var email = faker.Internet.Email(name);
        var registerRequest = new
        {
            name,
            email,
            password = "Teste@12345"
        };

        var registerContent = new StringContent
        (
            System.Text.Json.JsonSerializer.Serialize(registerRequest),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        await _httpClient.PostAsync("/api/user/", registerContent);

        var loginRequest = new
        {
            name,
            email,
            password = "123senha@."
        };

        var loginContent = new StringContent
        (
            System.Text.Json.JsonSerializer.Serialize(loginRequest),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync("api/Auth", loginContent);
        var json = await response.Content.ReadAsStringAsync();
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);


    }
}


