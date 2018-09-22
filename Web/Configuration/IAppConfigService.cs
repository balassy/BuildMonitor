using System.Collections.Generic;

namespace BuildMonitor.Web.Configuration
{
  public interface IAppConfigService
  {
    IReadOnlyList<DashboardConfig> Dashboards { get; }
  }
}
