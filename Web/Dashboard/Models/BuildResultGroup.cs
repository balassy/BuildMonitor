using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BuildMonitor.Web.Dashboard.Models
{
  public class BuildResultGroup
  {
    public string Title { get; set; }

    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Required for JSON serialization.")]
    public List<BuildResult> Builds { get; set; }
  }
}
