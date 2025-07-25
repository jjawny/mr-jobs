using System;

namespace MrJobs.WebApi.Features;

public class GlobalErrorTest
{
  public static string Handle() => throw new Exception("This is just a test, my APIs are bulletproof");

}
