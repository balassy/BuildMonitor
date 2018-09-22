using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace BuildMonitor.Web.Configuration
{
  public class AppConfigService : IAppConfigService
  {
    private readonly IConfiguration config;

    public AppConfigService(IConfiguration config)
    {
      this.config = config ?? throw new ArgumentNullException(nameof(config), "Please specify the runtime config for the AppConfigService!");
    }

    public IReadOnlyList<DashboardConfig> Dashboards => this.config.Get<AppConfig>().Dashboards.AsReadOnly();
  }
}
