using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace MrJobs.WebApi.Tests.Helpers;

public static class AuthHelpers
{
  public class AzAuthConfig(IConfiguration appSettings)
  {
    public string? TenantId { get; set; } = appSettings["AzAuth:TenantId"];
    public string? ClientId { get; set; } = appSettings["AzAuth:ClientId"];
    public string? ClientSecret { get; set; } = appSettings["AzAuth:ClientSecret"];
    public bool IsReady => !string.IsNullOrWhiteSpace(TenantId) &&
                           !string.IsNullOrWhiteSpace(ClientId) &&
                           !string.IsNullOrWhiteSpace(ClientSecret);
  }

  public static async Task<string?> GetRealAzureTokenAsync(string[] scopes, Microsoft.Extensions.Configuration.IConfiguration configuration)
  {
    var config = new AzAuthConfig(configuration);

    try
    {
      TokenCredential credential = config.IsReady
        // Use Client Credentials Flow with client secret
        ? credential = new ClientSecretCredential(
            config.TenantId,
            config.ClientId,
            config.ClientSecret
        // Use default methods (Managed Identity, Az CLI, etc)
        ) : new DefaultAzureCredential();

      var tokenRequestContext = new TokenRequestContext(scopes);
      var tokenResult = await credential.GetTokenAsync(tokenRequestContext, CancellationToken.None);

      return tokenResult.Token;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Failed to fetch real Azure token, reason: {ex.Message}");
      return null;
    }
  }
}
