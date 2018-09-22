using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace BuildMonitor.Web.Configuration
{
  public class AppConfigService : IAppConfigService
  {
    private readonly IOptionsSnapshot<AppConfig> config;

    public AppConfigService(IOptionsSnapshot<AppConfig> config)
    {
      this.config = config ?? throw new ArgumentNullException(nameof(config), "Please specify the runtime config for the AppConfigService!");
    }

    public IReadOnlyList<DashboardConfig> Dashboards => this.config.Value.Dashboards.AsReadOnly();
  }
}
