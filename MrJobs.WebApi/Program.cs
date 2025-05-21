using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using MrJobs.WebApi.Routes;

// Configure the DI container
var builder = WebApplication.CreateBuilder(args);
{
    var appSettings = builder.Configuration;

    builder.Services.AddOpenApi();

    builder.Services
      // .AddAuthentication().AddJwtBearer(options => appSettings.GetSection("Auth").Bind(options)); // JWT validation for generic providers (Firebase etc)
      .AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(appSettings.GetSection("Auth")); // JWT validation for Azure Entra ID
}

// Configure the HTTP request pipeline
var app = builder.Build();
{
    var isDevelopment = app.Environment.IsDevelopment();
    if (isDevelopment)
    {
        app.MapOpenApi();
    }

    var isProduction = app.Environment.IsProduction();
    if (isProduction)
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapGet("/ping", () => "healthy").AllowAnonymous();
    // Also test via generating a token via Postman to call this endpoint as a user
    app.MapGet("/poke", PokeRoute.Handle).RequireAuthorization();
    // Can read the key from Az Key Vault upon startup (if rotating, simply restart the web app to cache the new key in-mem)
    app.MapGet("/access-via-custom-api-key", ApiKeyRoute.Handle).AllowAnonymous();

    app.Run();
}
