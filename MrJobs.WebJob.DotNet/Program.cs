using System.Net;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Polly;

//   __      __      ___.         ____.     ___.    
//  /  \    /  \ ____\_ |__      |    | ____\_ |__  
//  \   \/\/   // __ \| __ \     |    |/  _ \| __ \ 
//   \        /\  ___/| \_\ \/\__|    (  <_> ) \_\ \
//    \__/\  /  \___  >___  /\________|\____/|___  /
//         \/       \/    \/                     \/ 
Console.WriteLine("Starting Job...");

try
{
  // 1. Read appsettings
  // TODO: logic/env check to read appsettings.Development.json
  var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
  var scope = configuration["WebApi:Scope"];
  var route = configuration["WebApi:Route"];

  if (string.IsNullOrWhiteSpace(scope))
    throw new ArgumentException("Bad API Scope");

  if (string.IsNullOrWhiteSpace(route))
    throw new ArgumentException("Bad API Route");

  Console.WriteLine($"Successfully read appsettings");

  // 2. Get the access token
  // Use `DefaultAzureCredential` >>> `ManagedIdentityCredential` as this object
  //  will attempt multiple auth methods in this order:
  //    1. Managed Identity (succeeds when running in Azure)
  //    2. az cli (succeeds when logged in locally)
  var credential = new DefaultAzureCredential(); // new ManagedIdentityCredential();
  var tokenRequestCtx = new Azure.Core.TokenRequestContext([$"{scope}/.default"]);
  var accessToken = await credential.GetTokenAsync(tokenRequestCtx);
  var jwt = accessToken.Token;

  Console.WriteLine($"Successfully obtained access token '{jwt[0..Math.Min(5, jwt.Length)]}...'");

  // 3. Make the HTTP request
  const int RETRY_COUNT = 3;
  static TimeSpan ExponentialBackoffInSeconds(int retryAttempt) => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));

  var retryPolicy = Policy
    .HandleResult<HttpResponseMessage>(result => result.StatusCode != HttpStatusCode.OK)
    .WaitAndRetryAsync(
        retryCount: RETRY_COUNT,
        sleepDurationProvider: retryAttempt => ExponentialBackoffInSeconds(retryAttempt),
        onRetry: (outcome, timespan, retryCount, context) =>
        {
          Console.WriteLine($"Retry {retryCount}/{RETRY_COUNT} after {timespan.Seconds}s");
        });

  var httpClient = new HttpClient()
  {
    DefaultRequestHeaders =
    {
      { "Authorization", $"Bearer {accessToken.Token}" },
    }
  };
  var uri = new Uri(route);
  var response = await retryPolicy.ExecuteAsync(async () => await httpClient.GetAsync(uri));
  var content = await response.Content.ReadAsStringAsync();

  if (response.StatusCode == HttpStatusCode.OK)
  {
    Console.WriteLine($"Job successful with content '{content}'");
  }
  else
  {
    Console.WriteLine($"Job failed with status code '{response.StatusCode}' and content '{content}'");
  }
}
catch (Exception ex)
{
  Console.WriteLine($"Job error, reason: {ex.Message}");
}
