using System;

namespace BuildMonitor.Web.Services
{
  public interface IDateConverter
  {
    string ConvertToHumanFriendlyString(DateTime timestamp, bool isUtcDate);
  }
}
