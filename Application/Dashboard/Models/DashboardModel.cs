using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BuildMonitor.Application.Dashboard.Models
{
  public class DashboardModel
  {
    public string Title { get; set; }

    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Required for JSON serialization.")]
    public List<GaugeGroupModel> Groups { get; set; }

    public string TimestampUtc { get; set; }
  }
}
