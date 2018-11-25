using BuildMonitor.Application.Converters;
using BuildMonitor.Application.Dashboard;
using BuildMonitor.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BuildMonitor.Application
{
  public static class Startup
  {
    public static void Configure(IServiceCollection services)
    {
      services.AddSingleton<IDateConverter, HumanizedDateConverter>();

      services.AddTransient<IDashboardService, DashboardService>();
    }
  }
}
