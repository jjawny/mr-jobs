using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using MrJobs.WebApi.Features;
using MrJobs.WebApi.Routes;

// Configure the DI container
var builder = WebApplication.CreateBuilder(args);
{
  var appSettings = builder.Configuration;
  var isDevelopment = builder.Environment.IsDevelopment();

  if (isDevelopment)
  {
  }
  else
  {
    // AZURE KEY VAULT EXAMPLE for OVERRIDING APPSETTINGS
    //  - See AzKeyVault > Access policies > Add access policy (allow web app to access the vault using Managed Identity)
    //  - See AzKeyVault > Objects > Secrets (populate secrets)
    //  - Gotcha: Colons are not supported in AzKeyVault, so use double dashes (--) for colons
    //            For example, secret "System--ApiKey" will override appsettings.json:
    //            {
    //              "System": {
    //                "ApiKey": "<OVERRIDEN>"
    //              }
    //            }

    // INACTIVE CODE as AzKeyVault has no free tier:
    // var azKeyVaultUri = new Uri(appSettings["AzKeyVault:Uri"]);
    // var secretsClient = new SecretClient(azKeyVaultUri, new DefaultAzureCredential());
    // appSettings.AddAzureKeyVault(secretsClient, new KeyVaultSecretManager());
  }

  builder.Services.AddOpenApi();

  builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(appSettings.GetSection("AzAuth"));
  builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
      .RequireAuthenticatedUser()
      .Build());
}

// Configure the HTTP request pipeline
var app = builder.Build();
{
  var isDevelopment = app.Environment.IsDevelopment();

  if (isDevelopment)
  {
    app.MapOpenApi();
  }
  else
  {
    app.UseHsts();
  }

  app.UseHttpsRedirection();
  app.UseExceptionHandler("/error");

  // Auth all routes by default (opt-out for anons)
  app.UseAuthentication();
  app.UseAuthorization();

  var healthCheck = app.MapGroup("/");
  healthCheck.MapGet("", HealthChecks.Handle).AllowAnonymous();
  healthCheck.MapGet("health", HealthChecks.Handle).AllowAnonymous();

  var globalError = app.MapGroup("/error");
  globalError.MapGet("", GlobalError.Handle).AllowAnonymous();
  globalError.MapGet("test", GlobalErrorTest.Handle).AllowAnonymous();

  var pokeFeatures = app.MapGroup("/poke");
  pokeFeatures.MapGet("", Poke.Handle);
  pokeFeatures.MapGet("using-api-key", PokeAsSystem.Handle).AllowAnonymous();

  app.Run();
}

// For integration tests to invoke the Web API (host in-memory)
public partial class Program { }
