using System;
using BuildMonitor.Services.Interfaces;

namespace BuildMonitor.Services.TeamCity
{
  public interface ITeamCityBuildCache
  {
    TestResults GetOrAddTestResults(string buildId, Func<TestResults> factory);
  }
}
