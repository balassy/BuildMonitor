using System;
using System.Collections.Generic;
using System.Linq;
using BuildMonitor.Services.Interfaces;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Fields;

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

      TeamCityClient client = new TeamCityClient(connectionParams.Host, useSsl: true);

      if (String.IsNullOrEmpty(connectionParams.Username))
      {
        client.ConnectAsGuest();
      }
      else
      {
        client.Connect(connectionParams.Username, connectionParams.Password);
      }

      // Add branch filter if specified.
      List<string> locatorParams = new List<string>();
      if (!String.IsNullOrEmpty(branchName))
      {
        locatorParams.Add($"branch:{branchName}");
      }

      // Define the list of fields that should be returned by the TeamCity API.
      BuildField buildFields = BuildField.WithFields(
        id: true,
        number: true,
        finishDate: true,
        status: true,
        statusText: true,
        triggered: TriggeredField.WithFields(
          type: true,
          details: true,
          userField: UserField.WithFields(
            name: true)),
        changes: ChangesField.WithFields(changeField: ChangeField.WithFields(
          id: true,
          date: true,
          username: true,
          userField: UserField.WithFields(
            name: true,
            username: true))));
      BuildsField buildsFields = BuildsField.WithFields(buildFields);

      // Get the last build of the specified build configuration on the given branch.
      Build build = client.Builds.GetFields(buildsFields.ToString()).LastBuildByBuildConfigId(buildConfigurationId, locatorParams);

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
        Status = TeamCityBuildService.GetStatus(build),
        TriggeredBy = TeamCityBuildService.GetTriggeredBy(build),
        LastChangeBy = TeamCityBuildService.GetLastChangeBy(build)
      };
    }

    private static BuildStatus GetStatus(Build build)
    {
      return build.Status.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase) ? BuildStatus.Success : BuildStatus.Failed;
    }

    private static string GetTriggeredBy(Build build)
    {
      if (build.Triggered.Type.Equals("vcs", StringComparison.OrdinalIgnoreCase))
      {
        return "Git";
      }

      string triggeredByUser = build.Triggered?.User?.Name;
      if (!string.IsNullOrEmpty(triggeredByUser))
      {
        return triggeredByUser;
      }

      return null;
    }

    private static string GetLastChangeBy(Build build)
    {
      Change lastChange = build.Changes.Change.OrderByDescending(c => c.Date).FirstOrDefault();
      if (lastChange == null)
      {
        return null;
      }

      string lastChangeUser = lastChange.User?.Name;
      if (!string.IsNullOrEmpty(lastChangeUser))
      {
        return lastChangeUser;
      }

      string lastChangeUsername = lastChange.Username;
      if (!string.IsNullOrEmpty(lastChangeUsername))
      {
        return lastChangeUsername;
      }

      return null;
    }
  }
}
