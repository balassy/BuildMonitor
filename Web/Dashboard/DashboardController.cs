using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BuildMonitor.Services.Interfaces;
using BuildMonitor.Web.Configuration;
using BuildMonitor.Web.Dashboard.Models;
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

    public DashboardController(IBuildService buildService, IAppConfigService config)
    {
      this.buildService = buildService ?? throw new ArgumentNullException(nameof(buildService), "Please specify the build service for the dashboard controller!");
      this.config = config ?? throw new ArgumentNullException(nameof(config), "Please specify the application configuration for the dashboard controller!");
    }

    [HttpGet("{slug}", Name = "Get")]
    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required by the runtime.")]
    public DashboardResultModel Get(string slug)
    {
      DashboardConfig dashboard = this.config.Dashboards.First(d => d.Slug == slug);

      BuildResult buildResult = this.buildService.GetLastBuildStatus("DummyBuildConfigurationId", "DummyBranchName");

      DashboardResultModel resultModel = new DashboardResultModel
      {
        Title = dashboard.Title,
        Groups = new List<BuildResultGroupModel>
        {
          new BuildResultGroupModel
          {
            Title = "Dummy builds",
            Builds = new List<BuildResultModel>
            {
              new BuildResultModel
              {
                Title = "Dummy result",
                BranchName = buildResult.BranchName,
                Status = buildResult.Status,
                TriggeredBy = buildResult.TriggeredBy
              }
            }
          },
          new BuildResultGroupModel
          {
            Title = "Frontend builds",
            Builds = new List<BuildResultModel>
            {
              new BuildResultModel { Title = "Component A" },
              new BuildResultModel { Title = "Component B" },
              new BuildResultModel { Title = "Component C" },
            }
          },
          new BuildResultGroupModel
          {
            Title = "Backend builds",
            Builds = new List<BuildResultModel>
            {
              new BuildResultModel { Title = "Service A" },
              new BuildResultModel { Title = "Service B" },
              new BuildResultModel { Title = "Service C" },
            }
          }
        }
      };

      return resultModel;
    }
  }
}
