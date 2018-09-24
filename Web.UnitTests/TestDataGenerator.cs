using System.Linq;
using Bogus;
using BuildMonitor.Services.Interfaces;
using BuildMonitor.Web.Configuration;

namespace BuildMonitor.Web.UnitTests
{
  internal static class TestDataGenerator
  {
    public static BuildResult GetBuildResult()
    {
      var result = new Faker<BuildResult>("en")
        .RuleFor(r => r.BranchName, f => f.Lorem.Word())
        .RuleFor(r => r.BuildId, f => f.Lorem.Word())
        .RuleFor(r => r.Status, f => f.PickRandom<BuildStatus>())
        .RuleFor(r => r.TriggeredBy, f => f.Person.FullName)
        .RuleFor(r => r.CompletedTimestamp, f => f.Date.Recent());
      return result.Generate();
    }

    public static DashboardConfig[] GetDashboardConfigs()
    {
      var buildConfig = new Faker<BuildConfig>()
        .RuleFor(r => r.Title, f => TestDataGenerator.GetTitle(f))
        .RuleFor(r => r.BranchName, f => TestDataGenerator.GetBranchName(f))
        .RuleFor(r => r.BuildConfigurationId, f => TestDataGenerator.GetBuildConfigurationId(f));

      var groupConfig = new Faker<GroupConfig>()
        .RuleFor(r => r.Title, f => TestDataGenerator.GetTitle(f))
        .RuleFor(r => r.Builds, f => buildConfig.Generate(3));

      var dashboardConfig = new Faker<DashboardConfig>()
        .RuleFor(r => r.Title, f => string.Join(" ", f.Lorem.Words()))
        .RuleFor(r => r.Slug, f => f.Lorem.Slug())
        .RuleFor(r => r.Groups, f => groupConfig.Generate(2));

      return dashboardConfig.Generate(3).ToArray();
    }

    private static string GetBranchName(Faker faker)
    {
      return faker.Lorem.Slug(wordcount: 3);
    }

    private static string GetBuildConfigurationId(Faker faker)
    {
      return faker.Lorem.Slug(wordcount: 4);
    }

    private static string GetTitle(Faker faker)
    {
      return string.Join(" ", faker.Lorem.Words());
    }
  }
}
