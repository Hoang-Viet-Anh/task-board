using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

public class RegistrationTests :
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program>
        _factory;

    public RegistrationTests(
        CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions());
    }

    [Fact]
    public async Task Post_RegisterAccount()
    {
        // Arrange
        var userData = new
        {
            Username = "user",
            Password = "password"
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/register")
        {
            Content = new StringContent(
                JsonSerializer.Serialize(userData),
                Encoding.UTF8,
                "application/json"
            )
        };

        //Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}