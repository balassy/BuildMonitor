using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BuildMonitor.Services.Interfaces;
using BuildMonitor.Web.Configuration;
using BuildMonitor.Web.Dashboard.Models;
using BuildMonitor.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildMonitor.Web.Dashboard
{
  [Route("api/[controller]")]
  [ApiController]
  [Produces("application/json")]
  public class DashboardController : ControllerBase
  {
    private readonly IBuildService buildService;
    private readonly IAppConfigService config;
    private readonly ITimestampConverter timestampConverter;

    public DashboardController(IBuildService buildService, IAppConfigService config, ITimestampConverter timestampConverter)
    {
      this.buildService = buildService ?? throw new ArgumentNullException(nameof(buildService), "Please specify the build service for the dashboard controller!");
      this.config = config ?? throw new ArgumentNullException(nameof(config), "Please specify the application configuration for the dashboard controller!");
      this.timestampConverter = timestampConverter ?? throw new ArgumentNullException(nameof(timestampConverter), "Please specify the timestamp converter for the dashboard controller!");
    }

    [HttpGet("{slug}", Name = "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required by the runtime.")]
    public ActionResult<DashboardResultModel> Get(string slug)
    {
      if (String.IsNullOrEmpty(slug))
      {
        return this.BadRequest("Please specify the dashboard!");
      }

      DashboardConfig dashboardConfig = this.GetDashboardConfig(slug);

      if (dashboardConfig == null)
      {
        return this.NotFound("The specified dashboard cannot be found!");
      }

      DashboardResultModel result = this.GetBuildResults(dashboardConfig);
      return result;
    }

    private DashboardConfig GetDashboardConfig(string slug)
    {
      return String.IsNullOrEmpty(slug)
        ? null
        : this.config.Dashboards.FirstOrDefault(d => d.Slug == slug);
    }

    private DashboardResultModel GetBuildResults(DashboardConfig dashboardConfig)
    {
      if (dashboardConfig == null)
      {
        throw new ArgumentNullException(nameof(dashboardConfig), "Please specify the dashboard configuration!");
      }

      DashboardResultModel dashboardResultModel = new DashboardResultModel
      {
        Title = dashboardConfig.Title,
        Groups = new List<BuildResultGroupModel>()
      };

      foreach (var groupConfig in dashboardConfig.Groups)
      {
        var groupResultModel = new BuildResultGroupModel
        {
          Title = groupConfig.Title,
          Builds = new List<BuildResultModel>()
        };

        foreach (var buildConfig in groupConfig.Builds)
        {
          BuildResult buildResult = this.buildService.GetLastBuildStatus(buildConfig.BuildConfigurationId, buildConfig.BranchName);

          var buildResultModel = new BuildResultModel
          {
            Title = buildConfig.Title,
            BranchName = buildConfig.BranchName,
            Status = buildResult.Status,
            TriggeredBy = buildResult.TriggeredBy,
            BuildId = buildResult.BuildId,
            CompletedTimestampHumanized = this.timestampConverter.ConvertToHumanFriendlyString(buildResult.CompletedTimestamp, isUtcDate: true)
          };

          groupResultModel.Builds.Add(buildResultModel);
        }

        dashboardResultModel.Groups.Add(groupResultModel);
      }

      return dashboardResultModel;
    }
  }
}
