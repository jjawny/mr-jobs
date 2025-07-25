using System;
using Microsoft.AspNetCore.Diagnostics;

namespace MrJobs.WebApi.Features;

public static class GlobalError
{
  /// <summary>
  /// A global catch just-in-case (for unhandled exceptions).
  /// These 500s are automatically captured in App Insights.
  /// We typically never reach here when using ErrOrValue pattern.
  ///   ^ This has ergonomic/performance/DX gains.
  /// </summary>
  public static IResult Handle(HttpContext context, IHostEnvironment env)
  {
    var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
    var userFriendlyMessage = exception?.Message ?? "Unexpected error";
    var extensions = new Dictionary<string, object?>
    {
      { "traceId", context.TraceIdentifier }
    };

    if (env.IsDevelopment())
    {
      extensions.Add("stackTrace", exception?.StackTrace);
    }

    var response = Results.Problem(
      statusCode: StatusCodes.Status500InternalServerError,
      title: "Something went wrong...",
      detail: userFriendlyMessage,
      extensions: extensions
    );

    return response;
  }
}
