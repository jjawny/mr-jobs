using System;

namespace MrJobs.WebApi.Routes;

public static class ApiKeyRoute
{
  public static IResult Handle(ClaimsPrincipal user)
  {
    var response = new
    {
      firstName = user.FindFirst(ClaimTypes.GivenName)?.Value,
      fullName = user.FindFirst(ClaimTypes.Name)?.Value,
      upn = user.FindFirst(ClaimTypes.Upn)?.Value,
      azObjectId = user.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value,
    };
    return Results.Ok(response);
  }
}
