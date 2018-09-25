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
    private readonly IDateConverter dateConverter;

    public DashboardController(IBuildService buildService, IAppConfigService config, IDateConverter dateConverter)
    {
      this.buildService = buildService ?? throw new ArgumentNullException(nameof(buildService), "Please specify the build service for the dashboard controller!");
      this.config = config ?? throw new ArgumentNullException(nameof(config), "Please specify the application configuration for the dashboard controller!");
      this.dateConverter = dateConverter ?? throw new ArgumentNullException(nameof(dateConverter), "Please specify the date converter for the dashboard controller!");
    }

    [HttpGet("{slug}", Name = "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required by the runtime.")]
    public ActionResult<DashboardModel> Get(string slug)
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

      DashboardModel result = this.GetBuildResults(dashboardConfig);
      return result;
    }

    private DashboardConfig GetDashboardConfig(string slug)
    {
      return String.IsNullOrEmpty(slug)
        ? null
        : this.config.Dashboards.FirstOrDefault(d => d.Slug == slug);
    }

    private DashboardModel GetBuildResults(DashboardConfig dashboardConfig)
    {
      if (dashboardConfig == null)
      {
        throw new ArgumentNullException(nameof(dashboardConfig), "Please specify the dashboard configuration!");
      }

      DashboardModel dashboardResultModel = new DashboardModel
      {
        Title = dashboardConfig.Title,
        Groups = new List<GaugeGroupModel>()
      };

      foreach (var groupConfig in dashboardConfig.Groups)
      {
        var gaugeGroupModel = new GaugeGroupModel
        {
          Title = groupConfig.Title,
          Gauges = new List<GaugeModel>()
        };

        foreach (var buildConfig in groupConfig.Builds)
        {
          BuildResult buildResult = this.buildService.GetLastBuildStatus(this.config.Connection, buildConfig.BuildConfigurationId, buildConfig.BranchName);

          if (buildResult != null)
          {
            var gaugeModel = new GaugeModel
            {
              Title = buildConfig.Title,
              BranchName = buildConfig.BranchName,
              Status = buildResult.Status,
              TriggeredBy = buildResult.TriggeredBy,
              BuildId = buildResult.BuildId,
              BuildNumber = buildResult.BuildNumber,
              FinishDateHumanized = this.dateConverter.ConvertToHumanFriendlyString(buildResult.FinishDate, isUtcDate: true)
            };

            gaugeGroupModel.Gauges.Add(gaugeModel);
          }
        }

        dashboardResultModel.Groups.Add(gaugeGroupModel);
      }

      return dashboardResultModel;
    }
  }
}
