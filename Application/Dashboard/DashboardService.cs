﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BuildMonitor.Application.Dashboard.Models;
using BuildMonitor.Application.Interfaces;
using BuildMonitor.Domain.Configuration;
using BuildMonitor.Domain.Entities;
using BuildMonitor.Domain.ValueObjects;

namespace BuildMonitor.Application.Dashboard
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

      if (this.config.Connection == null)
      {
        throw new ArgumentNullException(nameof(config), "Please specify the connection parameters in the application configuration!");
      }
    }

    public IReadOnlyList<DashboardConfig> GetDashboards()
    {
      return this.config.Dashboards;
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

      BuildServerConnection connection = new BuildServerConnection(this.config.Connection.Host, this.config.Connection.Username, this.config.Connection.Password);
      this.buildService.Connect(connection);

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
          ColumnCount = groupConfig.ColumnCount,
          Gauges = new List<GaugeModel>()
        };

        foreach (var buildConfig in groupConfig.Builds)
        {
          BuildResult buildResult = this.buildService.GetLastBuildStatus(buildConfig.BuildConfigurationId, buildConfig.BranchName);

          DateTime finishDate = buildResult.Status == BuildStatus.Running
            ? DateTime.Now
            : buildResult.FinishDate;

          if (buildResult != null)
          {
            var gaugeModel = new GaugeModel
            {
              Title = buildConfig.Title,
              BranchName = buildResult.BranchName,
              BuildId = buildResult.BuildId,
              BuildNumber = buildResult.BuildNumber,
              Status = buildResult.Status,
              TriggeredBy = buildResult.TriggeredBy,
              LastChangeBy = buildResult.LastChangeBy,
              FinishDateHumanized = this.dateConverter.ConvertToHumanFriendlyString(finishDate, isUtcDate: false),
              PassedTestCount = buildResult.Tests.PassedCount,
              FailedTestCount = buildResult.Tests.FailedCount,
              IgnoredTestCount = buildResult.Tests.IgnoredCount
            };

            gaugeGroupModel.Gauges.Add(gaugeModel);
          }
        }

        dashboardResultModel.Groups.Add(gaugeGroupModel);
      }

      dashboardResultModel.TimestampUtc = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);

      return dashboardResultModel;
    }
  }
}
