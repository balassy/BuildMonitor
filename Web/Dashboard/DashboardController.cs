using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BuildMonitor.Web.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuildMonitor.Web.Dashboard
{
  [Route("api/[controller]")]
  [ApiController]
  [Produces("application/json")]
  public class DashboardController : ControllerBase
  {
    [HttpGet("{id}", Name = "Get")]
    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required by the runtime.")]
    public DashboardResult Get(string id)
    {
      DashboardResult result = new DashboardResult
      {
        Id = id,
        Title = "My Application",
        Groups = new List<BuildResultGroup>
        {
          new BuildResultGroup
          {
            Title = "Frontend builds",
            Builds = new List<BuildResult>
            {
              new BuildResult { Title = "Component A" },
              new BuildResult { Title = "Component B" },
              new BuildResult { Title = "Component C" },
            }
          },
          new BuildResultGroup
          {
            Title = "Backend builds",
            Builds = new List<BuildResult>
            {
              new BuildResult { Title = "Service A" },
              new BuildResult { Title = "Service B" },
              new BuildResult { Title = "Service C" },
            }
          }
        }
      };

      return result;
    }
  }
}
