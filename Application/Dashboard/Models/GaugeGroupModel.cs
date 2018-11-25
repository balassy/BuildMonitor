using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BuildMonitor.Application.Dashboard.Models
{
  public class GaugeGroupModel
  {
    public string Title { get; set; }

    public int ColumnCount { get; set; }

    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Required for JSON serialization.")]
    public List<GaugeModel> Gauges { get; set; }
  }
}
