using System.Diagnostics.CodeAnalysis;

namespace BuildMonitor.Services.Interfaces
{
  /// <summary>
  /// The possible result statuses of a build.
  /// </summary>
  [SuppressMessage("Microsoft.Naming", "CA1717:OnlyFlagsEnumsShouldHavePluralNames", Justification = "False alarm.")]
  public enum BuildStatus
  {
    /// <summary>
    /// The build has been successfully completed.
    /// </summary>
    Success,

    /// <summary>
    /// The build is still running.
    /// </summary>
    Pending,

    /// <summary>
    /// The build is broken.
    /// </summary>
    Failed
  }
}
