using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildMonitor.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BuildMonitor.Services.TeamCity
{
  public static class Startup
  {
    public static void Configure(IServiceCollection services)
    {
      services.AddSingleton<IBuildService, TeamCityBuildService>();
    }
  }
}
