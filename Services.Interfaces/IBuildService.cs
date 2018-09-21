namespace BuildMonitor.Services.Interfaces
{
  /// <summary>
  /// Describes the expected features of a build server.
  /// </summary>
  public interface IBuildService
  {
    /// <summary>
    /// Returns the current status of the last build of the specified build configuration on the specified branch.
    /// </summary>
    /// <param name="buildConfigurationId">The unique identifier of the build configuration.</param>
    /// <param name="branchName">The name of the branch.</param>
    /// <returns>The status of the build.</returns>
    BuildResult GetLastBuildStatus(string buildConfigurationId, string branchName);
  }
}
