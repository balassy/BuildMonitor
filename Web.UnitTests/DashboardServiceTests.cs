using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using BuildMonitor.Services.Interfaces;
using BuildMonitor.Web.Configuration;
using BuildMonitor.Web.Dashboard;
using BuildMonitor.Web.Dashboard.Models;
using BuildMonitor.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BuildMonitor.Web.UnitTests
{
  [TestClass]
  public class DashboardServiceTests
  {
    private Mock<IBuildService> buildServiceMock;
    private Mock<IAppConfigService> configServiceMock;
    private DashboardService service;

    [TestInitialize]
    public void InitializeTest()
    {
      this.buildServiceMock = new Mock<IBuildService>();
      this.configServiceMock = new Mock<IAppConfigService>();
      var dateConverterMock = new Mock<IDateConverter>();
      dateConverterMock.Setup(m => m.ConvertToHumanFriendlyString(It.IsAny<DateTime>(), It.IsAny<bool>())).Returns(TestDataGenerator.GetFinishDateHumanized());

      this.service = new DashboardService(this.buildServiceMock.Object, this.configServiceMock.Object, dateConverterMock.Object);
    }

    [TestClass]
    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Pattern to structure tests.")]
    public class GetDashboardConfigMethod : DashboardServiceTests
    {
      [TestMethod]
      public void ShouldThrowIfSlugIsEmpty()
      {
        string slug = String.Empty;
        Assert.ThrowsException<ArgumentNullException>(() => this.service.GetDashboardConfig(slug));
      }

      [TestMethod]
      public void ShouldThrowIfSlugIsNull()
      {
        string slug = null;
        Assert.ThrowsException<ArgumentNullException>(() => this.service.GetDashboardConfig(slug));
      }

      [TestMethod]
      public void ShouldReturnTheMatchingDashboardConfiguration()
      {
        DashboardConfig[] testDashboardConfigs = TestDataGenerator.GetDashboardConfigs();
        this.configServiceMock.Setup(m => m.Dashboards).Returns(testDashboardConfigs);

        DashboardConfig testDashboardConfig = testDashboardConfigs[1];

        DashboardConfig result = this.service.GetDashboardConfig(testDashboardConfig.Slug);

        Assert.AreEqual(testDashboardConfig, result);
      }
    }

    [TestClass]
    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Pattern to structure tests.")]
    public class GetBuildResultsMethod : DashboardServiceTests
    {
      [TestMethod]
      public void ShouldThrowIfDashboardConfigurationIsNull()
      {
        DashboardConfig dashboardConfig = null;
        Assert.ThrowsException<ArgumentNullException>(() => this.service.GetBuildResults(dashboardConfig));
      }

      [TestMethod]
      public void ShouldReturnTheMatchingDashboardConfiguration()
      {
        // Generate and store random build results.
        BuildResult testBuildResult = TestDataGenerator.GetBuildResult();
        DashboardConfig[] testDashboardConfigs = TestDataGenerator.GetDashboardConfigs();

        var buildResults = new Dictionary<string, BuildResult>();

        DashboardConfig testDashboardConfig = testDashboardConfigs[1];
        foreach (var groupConfig in testDashboardConfig.Groups)
        {
          foreach (var buildConfig in groupConfig.Builds)
          {
            BuildResult buildResult = TestDataGenerator.GetBuildResult();
            buildResult.BranchName = buildConfig.BranchName;
            this.buildServiceMock.Setup(m => m.GetLastBuildStatus(buildConfig.BuildConfigurationId, buildConfig.BranchName)).Returns(buildResult);
            buildResults.Add($"{groupConfig.Title}-{buildConfig.Title}-{buildConfig.BuildConfigurationId}-{buildConfig.BranchName}", buildResult);
          }
        }

        ActionResult<DashboardModel> result = this.service.GetBuildResults(testDashboardConfig);

        this.buildServiceMock.Verify(m => m.Connect(It.IsAny<IConnectionParams>()), Times.Once());

        DashboardModel dashboardModel = result.Value;
        Assert.AreEqual(testDashboardConfig.Title, dashboardModel.Title);
        for (int groupIndex = 0; groupIndex < dashboardModel.Groups.Count; groupIndex++)
        {
          GroupConfig groupConfig = testDashboardConfig.Groups[groupIndex];
          GaugeGroupModel groupModel = dashboardModel.Groups[groupIndex];
          Assert.AreEqual(groupConfig.Title, groupModel.Title);

          for (int gaugeIndex = 0; gaugeIndex < groupModel.Gauges.Count; gaugeIndex++)
          {
            BuildConfig buildConfig = groupConfig.Builds[gaugeIndex];
            GaugeModel gaugeModel = groupModel.Gauges[gaugeIndex];
            Assert.AreEqual(buildConfig.Title, gaugeModel.Title);
            Assert.AreEqual(buildConfig.BranchName, gaugeModel.BranchName);

            BuildResult buildResult = buildResults[$"{groupConfig.Title}-{buildConfig.Title}-{buildConfig.BuildConfigurationId}-{buildConfig.BranchName}"];
            Assert.AreEqual(buildResult.Status, gaugeModel.Status);
            Assert.AreEqual(buildResult.BuildId, gaugeModel.BuildId);
            Assert.AreEqual(buildResult.BuildNumber, gaugeModel.BuildNumber);
            Assert.AreNotEqual(buildResult.FinishDate.ToString(CultureInfo.CurrentCulture), gaugeModel.FinishDateHumanized, "The finish date must be humanized!");
            Assert.AreEqual(buildResult.Tests.PassedCount, gaugeModel.PassedTestCount);
            Assert.AreEqual(buildResult.Tests.FailedCount, gaugeModel.FailedTestCount);
            Assert.AreEqual(buildResult.Tests.IgnoredCount, gaugeModel.IgnoredTestCount);
          }
        }
      }
    }
  }
}
