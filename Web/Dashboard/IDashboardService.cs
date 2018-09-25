using BuildMonitor.Web.Configuration;
using BuildMonitor.Web.Dashboard.Models;

namespace BuildMonitor.Web.Dashboard
{
  public interface IDashboardService
  {
    DashboardConfig GetDashboardConfig(string slug);

    DashboardModel GetBuildResults(DashboardConfig dashboardConfig);
  }
}
