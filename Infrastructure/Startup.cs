using BuildMonitor.Application.Interfaces;
using BuildMonitor.Infrastructure.TeamCity;
using Microsoft.Extensions.DependencyInjection;

namespace BuildMonitor.Infrastructure
{
  public static class Startup
  {
    public static void Configure(IServiceCollection services)
    {
      services.AddSingleton<IBuildService, TeamCityBuildService>();
      services.AddSingleton<ITeamCityBuildCache, TeamCityBuildCache>();
    }
  }
}
