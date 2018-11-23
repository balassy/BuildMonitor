using System;

namespace BuildMonitor.Application.Interfaces
{
  public interface IDateConverter
  {
    string ConvertToHumanFriendlyString(DateTime timestamp, bool isUtcDate);
  }
}
