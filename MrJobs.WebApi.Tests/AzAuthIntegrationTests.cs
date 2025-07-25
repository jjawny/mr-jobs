using System.Net.Http.Headers;
using MrJobs.WebApi.Tests.Helpers;
using MrJobs.WebApi.Tests.Factories;
using Microsoft.Extensions.Configuration;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace MrJobs.WebApi.Tests;

public class AzAuthIntegrationTests : IClassFixture<WebApiFactory>
{
  private readonly HttpClient _client;
  private readonly IConfiguration _appSettings;

  public AzAuthIntegrationTests(WebApiFactory factory)
  {
    _client = factory.CreateClient();
    _appSettings = factory.Services.GetRequiredService<IConfiguration>();
  }

  [Fact]
  public async Task Authorized_WithValidServicePrincipalToken_ReturnsOk()
  {
    // Arrange
    var clientId = _appSettings["AzAuth:ClientId"];
    var scopes = new[] { $"api://{clientId}/.default" };

    // Act, Assert
    var token = await AuthenticationTestHelper.GetRealAzureTokenAsync(scopes, _appSettings);
    Assert.False(string.IsNullOrEmpty(token), "Token acquisition failed, check appsettings.Testing.json");

    // Act, Assert
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    var response = await _client.GetAsync("/poke");
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }

  [Fact]
  public async Task Unauthorized_WithInvalidServicePrincipalToken_ReturnsUnauthorized()
  {
    // Arrange
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "bad-token");

    // Act
    var response = await _client.GetAsync("/poke");

    // Assert
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }
}
