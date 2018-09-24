using System;
using BuildMonitor.Services.Interfaces;

namespace BuildMonitor.Services.TeamCity
{
  /// <summary>
  /// The TeamCity specific implementation.
  /// </summary>
  public class TeamCityBuildService : IBuildService
  {
    public BuildResult GetLastBuildStatus(string buildConfigurationId, string branchName)
    {
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
