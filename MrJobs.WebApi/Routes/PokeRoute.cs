using System;
using System.Security.Claims;

namespace MrJobs.WebApi.Routes;

/// <summary>
/// An authorized endpoint that can be hit by Azure resources (via Managed Identity) or by users (via Azure Entra ID)
/// Test user POV by generating a JWT via Postman, then use the access token in the .http file
/// </summary>
public static class PokeRoute
{
  private static readonly string NONE = "none";
  public static IResult Handle(ClaimsPrincipal user)
  {
    var response = new
    {
      firstName = user.FindFirst(ClaimTypes.GivenName)?.Value ?? NONE,
      fullName = user.FindFirst(ClaimTypes.Name)?.Value ?? NONE,
      upn = user.FindFirst(ClaimTypes.Upn)?.Value ?? NONE,
      azObjectId = user.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value ?? NONE,
    };
    return Results.Ok(response);
  }
}
