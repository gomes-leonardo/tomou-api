using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Shouldly;

namespace Tomou.IntegrationTests;
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
        var email = $"leonardo{Guid.NewGuid()}@email.com";
        var request = new
        {
            name = "Leonardo",
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


        var response = await _httpClient.PostAsync("/api/user", content);
        var json = await response.Content.ReadAsStringAsync();
        json.ShouldContain("leonardo");
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenEmailAlreadyExists()
    {
        var email = $"leonardo{Guid.NewGuid()}@email.com";
        var request = new
        {
            name = "Leonardo",
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

        var firstResponse = await _httpClient.PostAsync("/api/user", content);
        firstResponse.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);

        var duplicatedContent = new StringContent
        (
            System.Text.Json.JsonSerializer.Serialize(request),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var secondResponse = await _httpClient.PostAsync("/api/user", duplicatedContent);
        secondResponse.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);


    }
}
