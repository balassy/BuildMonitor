using System;
using BuildMonitor.Application.Interfaces;
using Humanizer;

namespace BuildMonitor.Application.Converters
{
  public class HumanizedDateConverter : IDateConverter
  {
    public string ConvertToHumanFriendlyString(DateTime timestamp, bool isUtcDate)
    {
      return timestamp.Humanize(isUtcDate);
    }
  }
}
