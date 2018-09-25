using System;

namespace BuildMonitor.Services.Interfaces
{
  /// <summary>
  /// Describes the status of a given build.
  /// </summary>
  public class BuildResult
  {
    /// <summary>
    /// Gets or sets the unique internal identifier of the build (e.g. "1475634").
    /// </summary>
    public string BuildId { get; set; }

    /// <summary>
    /// Gets or sets the custom, public, visible identifier of the build (e.g. "1.4.13.1-beta").
    /// </summary>
    public string BuildNumber { get; set; }

    /// <summary>
    /// Gets or sets whether the build has completed successfully, failed or still running.
    /// </summary>
    public BuildStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the name of the branch the build is running on.
    /// </summary>
    public string BranchName { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the build has completed, or <c>null</c>, if the build is still running.
    /// </summary>
    public DateTime FinishDate { get; set; }

    /// <summary>
    /// Gets or sets the name of the person or service who initiated the build.
    /// </summary>
    public string TriggeredBy { get; set; }
  }
}
