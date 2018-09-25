using System;
using Humanizer;

namespace BuildMonitor.Web.Services
{
  public class HumanizedDateConverter : IDateConverter
  {
    public string ConvertToHumanFriendlyString(DateTime timestamp, bool isUtcDate)
    {
      return timestamp.Humanize(isUtcDate);
    }
  }
}
