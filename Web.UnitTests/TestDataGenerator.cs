using Bogus;
using BuildMonitor.Domain.Configuration;

namespace BuildMonitor.Web.UnitTests
{
  internal static class TestDataGenerator
  {
    public static DashboardConfig GetDashboardConfig()
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
        .RuleFor(r => r.Slug, f => TestDataGenerator.GetSlug())
        .RuleFor(r => r.Groups, f => groupConfig.Generate(2));

      return dashboardConfig;
    }

    public static string GetSlug()
    {
      return new Faker().Lorem.Slug();
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
