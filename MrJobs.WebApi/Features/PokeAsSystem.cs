using System;
using System.Net;
using ErrOrValue;
using Microsoft.AspNetCore.Mvc;

namespace MrJobs.WebApi.Routes;

/// <summary>
/// Route protected via a custom API key.
/// Used by the wider/system(s) that can't use Managed Identity.
/// </summary>
public static class PokeAsSystem
{
  public static IResult Handle([FromServices] IConfiguration appSettings, HttpRequest request)
  {
    var response = new ErrOr();

    if (!IsAuthorizedByApiKey(request, appSettings))
      return response.Set(
        message: "Unauthorized",
        severity: Severity.Error,
        code: HttpStatusCode.Unauthorized)
        .ToMinimalApiResponse();

    return response.Set(
      message: "Successfully authenticated",
      severity: Severity.Info,
      code: HttpStatusCode.OK)
      .ToMinimalApiResponse();
  }

  public static bool IsAuthorizedByApiKey(HttpRequest request, IConfiguration appSettings)
  {
    var expectedKey = appSettings["System:ApiKey"];
    var actualKey = request.Headers[appSettings["System:ApiKeyHeader"]!].FirstOrDefault();
    var isMatching = string.Equals(expectedKey, actualKey);
    return isMatching;
  }
}
