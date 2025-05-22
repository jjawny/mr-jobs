using System;
using Microsoft.AspNetCore.Mvc;

namespace MrJobs.WebApi.Routes;

/// <summary>
/// Example route that has authorization via a custom API key; used by other systems that can't use Managed Identity.
/// </summary>
public static class SystemJobRoute
{
  public static IResult Handle([FromServices] IConfiguration appSettings, HttpRequest request)
  {
    if (!IsAuthorizedByApiKey(request, appSettings))
      return Results.Unauthorized();

    return Results.Ok("Successfully authorized using custom API key");
  }

  public static bool IsAuthorizedByApiKey(HttpRequest request, IConfiguration appSettings)
  {
    var expectedKey = appSettings["SystemRoutes:ApiKey"];
    var actualKey = request.Headers[appSettings["SystemRoutes:ApiKeyHeader"]].FirstOrDefault();
    var isMatching = string.Equals(expectedKey, actualKey);
    return isMatching;
  }
}
