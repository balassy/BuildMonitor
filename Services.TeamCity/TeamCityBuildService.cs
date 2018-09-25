using System;
using BuildMonitor.Services.Interfaces;

namespace BuildMonitor.Services.TeamCity
{
  /// <summary>
  /// The TeamCity specific implementation.
  /// </summary>
  public class TeamCityBuildService : IBuildService
  {
    public BuildResult GetLastBuildStatus(IConnectionParams connectionParams, string buildConfigurationId, string branchName)
    {
      if (connectionParams == null)
      {
        throw new ArgumentNullException(nameof(connectionParams), "Please specify the connection parameters!");
      }

      return new BuildResult
      {
        BranchName = "Dummy branch name",
        BuildId = "Dummy666",
        CompletedTimestamp = DateTime.Now.AddHours(-5),
        Status = BuildStatus.Passed,
        TriggeredBy = "Git"
      };
    }
  }
}
