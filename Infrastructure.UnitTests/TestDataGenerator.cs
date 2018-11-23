using System;
using Bogus;

namespace BuildMonitor.Services.TeamCity.UnitTests
{
  internal static class TestDataGenerator
  {
    public static string GetBuildId()
    {
      return new Faker().Lorem.Word();
    }
  }
}
