using System;
using Humanizer;

namespace BuildMonitor.Web.Services
{
  public class HumanizerTimestampConverter : ITimestampConverter
  {
    public string ConvertToHumanFriendlyString(DateTime timestamp, bool isUtcDate)
    {
      return timestamp.Humanize(isUtcDate);
    }
  }
}
