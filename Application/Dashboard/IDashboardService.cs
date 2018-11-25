using System.Collections.Generic;
using BuildMonitor.Application.Dashboard.Models;
using BuildMonitor.Domain.Configuration;

namespace BuildMonitor.Application.Dashboard
{
  public interface IDashboardService
  {
    IReadOnlyList<DashboardConfig> GetDashboards();

    DashboardConfig GetDashboardConfig(string slug);

    DashboardModel GetBuildResults(DashboardConfig dashboardConfig);
  }
}
