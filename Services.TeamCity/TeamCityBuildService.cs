using System;
using System.Collections.Generic;
using BuildMonitor.Services.Interfaces;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;

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

      if (String.IsNullOrEmpty(buildConfigurationId))
      {
        throw new ArgumentNullException(nameof(buildConfigurationId), "Please specify the build configuration ID!");
      }

      if (String.IsNullOrEmpty(branchName))
      {
        throw new ArgumentNullException(nameof(branchName), "Please specify the name of the branch!");
      }

      var client = new TeamCityClient(connectionParams.Host, useSsl: true);
      client.Connect(connectionParams.Username, connectionParams.Password);

      var locatorParams = new List<string>
      {
        $"branch:{branchName}"
      };
      Build build = client.Builds.LastBuildByBuildConfigId(buildConfigurationId, locatorParams);

      if (build == null)
      {
        return null;
      }

      return new BuildResult
      {
        BranchName = branchName,
        BuildId = build.Id,
        CompletedTimestamp = build.FinishDate,
        Status = build.Status.Equals("passed", StringComparison.OrdinalIgnoreCase) ? BuildStatus.Passed : BuildStatus.Failed,
        TriggeredBy = build.Triggered.User.Name
      };
    }
  }
}
