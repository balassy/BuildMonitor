using System.Collections.Generic;
using BuildMonitor.Domain.Configuration;

namespace BuildMonitor.Application.Interfaces
{
  public interface IAppConfigService
  {
    IReadOnlyList<DashboardConfig> Dashboards { get; }

    ConnectionConfig Connection { get; }
  }
}
