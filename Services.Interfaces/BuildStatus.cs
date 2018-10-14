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
    /// The status is pending, because the build is still running.
    /// </summary>
    Running,

    /// <summary>
    /// The build is broken.
    /// </summary>
    Failed
  }
}
