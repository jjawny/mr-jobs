using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MrJobs.WebApi.Tests.Factories;

namespace MrJobs.WebApi.Tests;

public class SystemAuthIntegrationTests : IClassFixture<WebApiFactory>
{
  private readonly HttpClient _httpClient;
  private readonly IConfiguration _appSettings;

  public SystemAuthIntegrationTests(WebApiFactory factory)
  {
    _httpClient = factory.CreateClient();
    _appSettings = factory.Services.GetRequiredService<IConfiguration>();
  }

  [Fact]
  public async Task Authorized_WithValidApiKey_ReturnsOk()
  {
    // Arrange
    var headerName = _appSettings["System:ApiKeyHeader"];
    var apiKey = _appSettings["System:ApiKey"];
    _httpClient.DefaultRequestHeaders.Add(headerName, apiKey);

    // Act
    var response = await _httpClient.GetAsync("/poke/using-api-key");

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }

  [Fact]
  public async Task Unauthorized_WithInvalidApiKey_ReturnsUnauthorized()
  {
    // Arrange
    var headerName = _appSettings["System:ApiKeyHeader"];
    _httpClient.DefaultRequestHeaders.Add(headerName, "invalid-key");

    // Act
    var response = await _httpClient.GetAsync("/poke/using-api-key");

    // Assert
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }
}
