using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BuildMonitor.Application.Dashboard;
using BuildMonitor.Application.Dashboard.Models;
using BuildMonitor.Domain.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildMonitor.Web.Controllers
{
  [ApiController]
  [Route("api/dashboards")]
  [Produces("application/json")]
  public class DashboardController : ControllerBase
  {
    private readonly IDashboardService service;

    public DashboardController(IDashboardService service)
    {
      this.service = service ?? throw new ArgumentNullException(nameof(service), "Please specify the service for the dashboard controller!");
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<List<DashboardLinkModel>> Get()
    {
      IReadOnlyList<DashboardConfig> dashboards = this.service.GetDashboards();
      IEnumerable<DashboardLinkModel> dashboardLinks = dashboards.Select(d => new DashboardLinkModel
      {
        Title = d.Title,
        Slug = d.Slug
      });

      return dashboardLinks.ToList();
    }

    [HttpGet("{slug}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required by the runtime.")]
    public ActionResult<DashboardModel> Get(string slug)
    {
      if (String.IsNullOrEmpty(slug))
      {
        return this.BadRequest("Please specify the dashboard!");
      }

      DashboardConfig dashboardConfig = this.service.GetDashboardConfig(slug);

      if (dashboardConfig == null)
      {
        return this.NotFound("The specified dashboard cannot be found!");
      }

      DashboardModel result = this.service.GetBuildResults(dashboardConfig);
      return result;
    }
  }
}
