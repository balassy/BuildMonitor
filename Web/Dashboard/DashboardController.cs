using System;
using System.Diagnostics.CodeAnalysis;
using BuildMonitor.Web.Configuration;
using BuildMonitor.Web.Dashboard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildMonitor.Web.Dashboard
{
  [Route("api/[controller]")]
  [ApiController]
  [Produces("application/json")]
  public class DashboardController : ControllerBase
  {
    private readonly IDashboardService service;

    public DashboardController(IDashboardService service)
    {
      this.service = service ?? throw new ArgumentNullException(nameof(service), "Please specify the service for the dashboard controller!");
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
