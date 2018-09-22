using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BuildMonitor.Web.Configuration
{
  public class DashboardConfig
  {
    public string Slug { get; set; }

    public string Title { get; set; }

    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Required for JSON deserialization.")]
    public List<GroupConfig> Groups { get; set; }
  }
}
