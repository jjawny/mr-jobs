using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
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
    // OVERRIDE APPSETTINGS with VALUES from AZURE KEY VAULT
    //  - See AzKeyVault > Access policies > Add access policy (allow web app to access the vault using Managed Identity)
    //  - See AzKeyVault > Objects > Secrets (define secrets)
    //  - GOTCHA: Colons are not supported in AzKeyVault, so use double dashes (--) for colons
    //  - EXAMPLE: for a secret named "SystemRoutes--ApiKey" translates to "SystemRoutes:ApiKey" which will override
    //    {
    //      "SystemRoutes": {
    //        "ApiKey": "<OVERRIDEN>"
    //      }
    //    }

    // var azKeyVaultUri = new Uri(appSettings["AzKeyVault:Uri"]);
    // var client = new SecretClient(azKeyVaultUri, new DefaultAzureCredential());
    // appSettings.AddAzureKeyVault(client, new KeyVaultSecretManager());
  }

  builder.Services.AddOpenApi();

  builder.Services
  //.AddAuthentication().AddJwtBearer(options => appSettings.GetSection("Auth").Bind(options)); // JWT validation for generic providers (Firebase etc)
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(appSettings.GetSection("Auth")); // JWT validation for Azure Entra ID

  // RequireAuthorization on all routes by default (opt-out using AllowAnonymous)
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

  app.UseAuthentication();
  app.UseAuthorization();

  app.MapGet("/ping", () => "healthy").AllowAnonymous();
  app.MapGet("/poke", PokeRoute.Handle);
  app.MapGet("/access-via-custom-api-key", SystemJobRoute.Handle).AllowAnonymous();

  app.Run();
}
