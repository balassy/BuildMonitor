using System.Collections.Generic;
using BuildMonitor.Web.Configuration;
using BuildMonitor.Web.Dashboard.Models;

namespace BuildMonitor.Web.Dashboard
{
  public interface IDashboardService
  {
    IReadOnlyList<DashboardConfig> GetDashboards();

    DashboardConfig GetDashboardConfig(string slug);

    DashboardModel GetBuildResults(DashboardConfig dashboardConfig);
  }
}
