using Bogus;
using BuildMonitor.Services.Interfaces;

namespace BuildMonitor.Web.UnitTests
{
  internal static class TestDataGenerator
  {
    public static BuildResult GetBuildResult()
    {
      var result = new Faker<BuildResult>("en")
        .RuleFor(r => r.BranchName, f => f.Lorem.Word())
        .RuleFor(r => r.Id, f => f.Lorem.Word())
        .RuleFor(r => r.Status, f => f.PickRandom<BuildStatus>())
        .RuleFor(r => r.TriggeredBy, f => f.Person.FullName);
      return result.Generate();
    }

    public static string GetDashboardId()
    {
      return new Faker().Lorem.Slug();
    }
  }
}
