using System;
using BuildMonitor.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BuildMonitor.Web.Dashboard.Models
{
  public class BuildResultModel
  {
    public string Title { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public BuildStatus Status { get; set; }

    public string BuildId { get; set; }

    public string BuildNumber { get; set; }

    public string FinishDateHumanized { get; set; }

    public string BranchName { get; set; }

    public string TriggeredBy { get; set; }
  }
}
