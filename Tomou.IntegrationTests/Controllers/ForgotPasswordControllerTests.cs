using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace Tomou.IntegrationTests.Controllers;
public class ForgotPasswordControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public ForgotPasswordControllerTests(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task ForgotPassword_ReturnsNoContent_WhenEmailExists()
    {
        var faker = new Faker("pt_BR");
        var name = faker.Name.FullName();
        var email = faker.Internet.Email(name);
        var registerRequest = new
        {
            name,
            email,
            password = "Teste@12345!"
        };

        var registerContent = new StringContent
       (
           System.Text.Json.JsonSerializer.Serialize(registerRequest),
           System.Text.Encoding.UTF8,
           "application/json"
       );

        await _httpClient.PostAsync("/api/user", registerContent);

        var forgotPasswordRequest = new
        {
            registerRequest.email,
        };

        var forgotPasswordContent = new StringContent
       (
          System.Text.Json.JsonSerializer.Serialize(forgotPasswordRequest),
          System.Text.Encoding.UTF8,
          "application/json"
       );

        var forgotPasswordResponse = await _httpClient.PostAsync("api/ForgotPassword", forgotPasswordContent);
        forgotPasswordResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}