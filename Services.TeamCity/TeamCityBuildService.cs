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

      TeamCityClient client = new TeamCityClient(connectionParams.Host, useSsl: true);

      var locatorParams = new List<string>
      if (String.IsNullOrEmpty(connectionParams.Username))
      {
        client.ConnectAsGuest();
      }
      else
      {
        client.Connect(connectionParams.Username, connectionParams.Password);
      }
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
        BuildNumber = build.Number,
        FinishDate = build.FinishDate,
        Status = build.Status.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase) ? BuildStatus.Success : BuildStatus.Failed,
        TriggeredBy = build.Triggered?.User?.Name
      };
    }
  }
}
