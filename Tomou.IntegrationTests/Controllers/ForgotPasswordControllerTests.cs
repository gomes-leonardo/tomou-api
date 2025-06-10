using Bogus;
using Bogus.DataSets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Text;
using Tomou.Infrastructure.DataAccess;

namespace Tomou.IntegrationTests.Controllers;
public class ForgotPasswordControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    private readonly WebApplicationFactory<Program> _factory; 

    public ForgotPasswordControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;

        _httpClient = _factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
        }).CreateClient();
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

        await _httpClient.PostAsync("/api/user/register", registerContent);

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

        var forgotPasswordResponse = await _httpClient.PostAsync("forgot-password", forgotPasswordContent);
        forgotPasswordResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ForgotPassword_ReturnsNoContent_WhenEmailDoesNotExist()
    {
        var faker = new Faker("pt_BR");
        var email = faker.Internet.Email();

        var forgotPasswordRequest = new
        {
            email,
        };

        var forgotPasswordContent = new StringContent
      (
         System.Text.Json.JsonSerializer.Serialize(forgotPasswordRequest),
         System.Text.Encoding.UTF8,
         "application/json"
      );

        var forgotPasswordResponse = await _httpClient.PostAsync("/forgot-password", forgotPasswordContent);
        forgotPasswordResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

}