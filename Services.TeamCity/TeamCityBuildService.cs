using System;
using System.Collections.Generic;
using System.Globalization;
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
    private readonly ITeamCityBuildCache cache;

    private ITeamCityClient client;

    private bool isConnected;

    public TeamCityBuildService(ITeamCityBuildCache cache)
    {
      this.cache = cache ?? throw new ArgumentNullException(nameof(cache), "Please specify the memory cache for the TeamCity build service!");
    }

    public void Connect(IConnectionParams connectionParams)
    {
      if (connectionParams == null)
      {
        throw new ArgumentNullException(nameof(connectionParams), "Please specify the connection parameters!");
      }

      this.client = new TeamCityClient(connectionParams.Host, useSsl: true);

      if (String.IsNullOrEmpty(connectionParams.Username))
      {
        this.client.ConnectAsGuest();
      }
      else
      {
        this.client.Connect(connectionParams.Username, connectionParams.Password);
      }

      this.isConnected = true;
    }

    public BuildResult GetLastBuildStatus(string buildConfigurationId, string branchName)
    {
      if (String.IsNullOrEmpty(buildConfigurationId))
      {
        throw new ArgumentNullException(nameof(buildConfigurationId), "Please specify the build configuration ID!");
      }

      this.AssertClientConnected();

      Build build = this.GetLastBuild(buildConfigurationId, branchName);

      if (build == null)
      {
        return null;
      }

      TestResults testResults = this.GetTestResults(build.Id);

      return new BuildResult
      {
        BranchName = String.IsNullOrEmpty(branchName) ? "(default)" : branchName,
        BuildId = build.Id,
        BuildNumber = build.Number,
        FinishDate = build.FinishDate,
        Status = TeamCityBuildService.GetStatus(build),
        TriggeredBy = TeamCityBuildService.GetTriggeredBy(build),
        LastChangeBy = TeamCityBuildService.GetLastChangeBy(build),
        Tests = testResults
      };
    }

    private static BuildStatus GetStatus(Build build)
    {
      return build.State.Equals("running", StringComparison.OrdinalIgnoreCase)
        ? BuildStatus.Running
        : build.Status.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase)
          ? BuildStatus.Success
          : BuildStatus.Failed;
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

    private Build GetLastBuild(string buildConfigurationId, string branchName)
    {
      if (String.IsNullOrEmpty(buildConfigurationId))
      {
        throw new ArgumentNullException(nameof(buildConfigurationId), "Please specify the build configuration ID!");
      }

      this.AssertClientConnected();

      List<string> locatorParams = new List<string>();

      // Ensure that running builds are also included.
      locatorParams.Add("running:any");

      // Add branch filter if specified.
      if (!String.IsNullOrEmpty(branchName))
      {
        locatorParams.Add($"branch:{branchName}");
      }

      // Define the list of fields that should be returned by the TeamCity API.
      BuildField buildFields = BuildField.WithFields(
        id: true,
        number: true,
        href: true,
        webUrl: true,
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
      Build build = this.client.Builds.GetFields(buildsFields.ToString()).LastBuildByBuildConfigId(buildConfigurationId, locatorParams);

      return build;
    }

    private TestResults GetTestResults(string buildId)
    {
      if (String.IsNullOrEmpty(buildId))
      {
        throw new ArgumentNullException(nameof(buildId), "Please specify the build ID!");
      }

      this.AssertClientConnected();

      TestResults results = this.cache.GetOrAddTestResults(buildId, factory: () =>
      {
        TestResults newResults = new TestResults();
        List<Property> statistics = this.client.Statistics.GetByBuildId(buildId);
        foreach (Property property in statistics)
        {
          if (property.Name == "PassedTestCount")
          {
            newResults.PassedCount = Int32.Parse(property.Value, CultureInfo.InvariantCulture);
          }

          if (property.Name == "FailedTestCount")
          {
            newResults.FailedCount = Int32.Parse(property.Value, CultureInfo.InvariantCulture);
          }

          if (property.Name == "IgnoredTestCount")
          {
            newResults.IgnoredCount = Int32.Parse(property.Value, CultureInfo.InvariantCulture);
          }
        }
        return newResults;
      });

      return results;
    }

    private void AssertClientConnected()
    {
      if (!this.isConnected)
      {
        throw new InvalidOperationException("Please connect before doing this action!");
      }
    }
  }
}
