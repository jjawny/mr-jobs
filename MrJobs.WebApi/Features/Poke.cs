using System;
using System.Net;
using System.Security.Claims;
using ErrOrValue;

namespace MrJobs.WebApi.Routes;

/// <summary>
/// A traditional authed endpoint that can be hit by Azure resources (Managed Identity) or by endusers ("Sign in with Microsoft")
/// </summary>
public static class Poke
{
  public static IResult Handle(ClaimsPrincipal user)
  {
    var response = new ErrOr<object>();

    var claims = new
    {
      azOid = user.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value,
      first = user.FindFirst(ClaimTypes.GivenName)?.Value,
      last = user.FindFirst(ClaimTypes.Surname)?.Value,
      full = user.FindFirst(ClaimTypes.Name)?.Value,
      email = user.FindFirst(ClaimTypes.Email)?.Value,
      upn = user.FindFirst(ClaimTypes.Upn)?.Value,
      scope = user.FindFirst("http://schemas.microsoft.com/identity/claims/scope")?.Value,
      ip = user.FindFirst("ipaddr")?.Value,
    };

    return response.Set(
      message: "Successfully authenticated",
      severity: Severity.Info,
      code: HttpStatusCode.OK,
      value: claims)
      .ToMinimalApiResponse();
  }
}
