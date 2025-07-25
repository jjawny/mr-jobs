using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MrJobs.WebApi.Tests.Factories;

public class WebApiFactory : WebApplicationFactory<Program>
{
  protected override IHost CreateHost(IHostBuilder builder)
  {
    builder.UseEnvironment("Testing");
    builder.ConfigureAppConfiguration((context, config) =>
    {
      config.Sources.Clear();
      config.AddJsonFile("appsettings.Testing.json", optional: false);
    });

    return base.CreateHost(builder);
  }
}
