using BuildMonitor.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BuildMonitor.Services.TeamCity
{
  public static class Startup
  {
    public static void Configure(IServiceCollection services)
    {
      Startup.ConfigureExternalInterfaces(services);
      Startup.ConfigureInternalInterfaces(services);
    }

    public static void ConfigureExternalInterfaces(IServiceCollection services)
    {
      services.AddSingleton<IBuildService, TeamCityBuildService>();
    }

    public static void ConfigureInternalInterfaces(IServiceCollection services)
    {
      services.AddSingleton<ITeamCityBuildCache, TeamCityBuildCache>();
    }
  }
}
