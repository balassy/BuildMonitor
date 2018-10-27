using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BuildMonitor.Web.Configuration
{
  public class GroupConfig
  {
    public string Title { get; set; }

    public int ColumnCount { get; set; }

    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Required for JSON deserialization.")]
    public List<BuildConfig> Builds { get; set; }
  }
}
