using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BuildMonitor.Web.Dashboard.Models
{
  public class DashboardResultModel
  {
    public string Id { get; set; }

    public string Title { get; set; }

    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Required for JSON serialization.")]
    public List<BuildResultGroupModel> Groups { get; set; }
  }
}
