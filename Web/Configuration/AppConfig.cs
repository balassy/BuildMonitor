using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BuildMonitor.Web.Configuration
{
  public class AppConfig
  {
    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Required for JSON deserialization.")]
    public List<DashboardConfig> Dashboards { get; set; }
  }
}
