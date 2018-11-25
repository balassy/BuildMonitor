using System;
using BuildMonitor.Domain.Entities;

namespace BuildMonitor.Infrastructure.TeamCity
{
  public interface ITeamCityBuildCache
  {
    TestRunResult GetOrAddTestResult(string buildId, Func<TestRunResult> factory);
  }
}
