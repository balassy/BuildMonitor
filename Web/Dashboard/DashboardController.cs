using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BuildMonitor.Services.Interfaces;
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

    public DashboardController(IBuildService buildService)
    {
      this.buildService = buildService ?? throw new ArgumentException("Please specify the build service for the dashboard controller!", nameof(buildService));
    }

    [HttpGet("{dashboardId}", Name = "Get")]
    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required by the runtime.")]
    public DashboardResultModel Get(string dashboardId)
    {
      BuildResult buildResult = this.buildService.GetLastBuildStatus("DummyBuildConfigurationId", "DummyBranchName");

      DashboardResultModel resultModel = new DashboardResultModel
      {
        Id = dashboardId,
        Title = "My Application",
        Groups = new List<BuildResultGroupModel>
        {
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
          },
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
          }
        }
      };

      return resultModel;
    }
  }
}
