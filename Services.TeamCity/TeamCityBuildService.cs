using System;
using BuildMonitor.Services.Interfaces;

namespace BuildMonitor.Services.TeamCity
{
  /// <summary>
  /// My sample class.
  /// </summary>
  public class TeamCityBuildService : IBuildService
  {
    public BuildResult GetLastBuildStatus(string buildConfigurationId, string branchName)
    {
      return new BuildResult
      {
        BranchName = "Dummy branch name",
        BuildId = "Dummy666",
        CompletedTimestamp = DateTime.Now,
        Status = BuildStatus.Passed,
        TriggeredBy = "Git"
      };
    }
  }
}
