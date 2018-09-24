using System;

namespace BuildMonitor.Web.Services
{
  public interface ITimestampConverter
  {
    string ConvertToHumanFriendlyString(DateTime timestamp, bool isUtcDate);
  }
}
