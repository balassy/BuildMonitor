using System;
using System.Collections.Generic;
using System.Linq;
using BuildMonitor.Services.Interfaces;
using BuildMonitor.Web.Configuration;
using BuildMonitor.Web.Dashboard.Models;
using BuildMonitor.Web.Services;

namespace BuildMonitor.Web.Dashboard
{
  public class DashboardService : IDashboardService
  {
    private readonly IBuildService buildService;
    private readonly IAppConfigService config;
    private readonly IDateConverter dateConverter;

    public DashboardService(IBuildService buildService, IAppConfigService config, IDateConverter dateConverter)
    {
      this.buildService = buildService ?? throw new ArgumentNullException(nameof(buildService), "Please specify the build service for the dashboard controller!");
      this.config = config ?? throw new ArgumentNullException(nameof(config), "Please specify the application configuration for the dashboard controller!");
      this.dateConverter = dateConverter ?? throw new ArgumentNullException(nameof(dateConverter), "Please specify the date converter for the dashboard controller!");
    }

    public DashboardConfig GetDashboardConfig(string slug)
    {
      if (String.IsNullOrEmpty(slug))
      {
        throw new ArgumentNullException(nameof(slug), "Please specify the slug to find the dashboard configuration!");
      }

      return this.config.Dashboards.FirstOrDefault(d => d.Slug == slug);
    }

    public DashboardModel GetBuildResults(DashboardConfig dashboardConfig)
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
