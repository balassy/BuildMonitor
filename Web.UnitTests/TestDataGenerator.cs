using System.Globalization;
using Bogus;
using BuildMonitor.Domain.Configuration;
using BuildMonitor.Domain.Entities;

namespace BuildMonitor.Web.UnitTests
{
  internal static class TestDataGenerator
  {
    public static BuildResult GetBuildResult()
    {
      var testResult = new Faker<TestRunResult>()
        .RuleFor(r => r.PassedCount, f => f.Random.Int())
        .RuleFor(r => r.FailedCount, f => f.Random.Int())
        .RuleFor(r => r.IgnoredCount, f => f.Random.Int());

      var buildResult = new Faker<BuildResult>("en")
        .RuleFor(r => r.BranchName, f => f.Lorem.Word())
        .RuleFor(r => r.BuildId, f => f.Lorem.Word())
        .RuleFor(r => r.Status, f => f.PickRandom<BuildStatus>())
        .RuleFor(r => r.TriggeredBy, f => f.Person.FullName)
        .RuleFor(r => r.FinishDate, f => f.Date.Recent())
        .RuleFor(r => r.Tests, f => testResult.Generate());

      return buildResult.Generate();
    }

    public static ConnectionConfig GetConnectionConfig()
    {
      return new Faker<ConnectionConfig>()
        .RuleFor(r => r.Host, f => f.Internet.DomainName())
        .RuleFor(r => r.Username, f => f.Internet.UserName())
        .RuleFor(r => r.Password, f => f.Internet.Password())
        .Generate();
    }

    public static DashboardConfig GetDashboardConfig()
    {
      return TestDataGenerator.GetDashboardConfigFaker().Generate();
    }

    public static DashboardConfig[] GetDashboardConfigs()
    {
      return TestDataGenerator.GetDashboardConfigFaker().Generate(3).ToArray();
    }

    public static string GetFinishDateHumanized()
    {
      return new Faker().Date.Past().ToString(CultureInfo.CurrentCulture);
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

    private static Faker<DashboardConfig> GetDashboardConfigFaker()
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
  }
}
