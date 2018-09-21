using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace BuildMonitor.Web.Controllers
{
  [Route("api/[controller]")]
  public class SampleDataController : Controller
  {
    private static string[] summaries = new[]
    {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet("[action]")]
    public IEnumerable<WeatherForecast> WeatherForecasts()
    {
      Random rng = new Random();
      return Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        DateFormatted = DateTime.Now.AddDays(index).ToString("d", CultureInfo.InvariantCulture),
        TemperatureC = rng.Next(-20, 55),
        Summary = summaries[rng.Next(summaries.Length)]
      });
    }

#pragma warning disable CA1034 // NestedTypesShouldNotBeVisible
    public class WeatherForecast
    {
      public string DateFormatted { get; set; }

      public int TemperatureC { get; set; }

      public string Summary { get; set; }

      public int TemperatureF => 32 + (int)(this.TemperatureC / 0.5556);
    }
#pragma warning restore CA1034 // NestedTypesShouldNotBeVisible
  }
}
