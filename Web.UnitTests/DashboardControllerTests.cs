using System;
using System.Collections.Generic;
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
  public class DashboardControllerTests
  {
    private DashboardController controller;

    private BuildResult testBuildResult;

    private DashboardConfig[] testDashboardConfigs;

    private Mock<IBuildService> buildServiceMock;

    [TestInitialize]
    public void InitializeTest()
    {
      this.testBuildResult = TestDataGenerator.GetBuildResult();
      this.testDashboardConfigs = TestDataGenerator.GetDashboardConfigs();

      var configServiceMock = new Mock<IAppConfigService>();
      configServiceMock.Setup(m => m.Dashboards).Returns(this.testDashboardConfigs);

      var timestampConverterMock = new Mock<IDateConverter>();
      timestampConverterMock.Setup(m => m.ConvertToHumanFriendlyString(It.IsAny<DateTime>(), It.IsAny<bool>())).Returns(TestDataGenerator.GetFinishDate());

      this.buildServiceMock = new Mock<IBuildService>();
      this.buildServiceMock.Setup(m => m.GetLastBuildStatus(It.IsAny<IConnectionParams>(), It.IsAny<string>(), It.IsAny<string>())).Returns(this.testBuildResult);
      this.controller = new DashboardController(this.buildServiceMock.Object, configServiceMock.Object, timestampConverterMock.Object);
    }

    [TestMethod]
    public void ShouldReturnDashboardResultFromTheService()
    {
      var buildResults = new Dictionary<string, BuildResult>();

      DashboardConfig testDashboardConfig = this.testDashboardConfigs[1];
      foreach (var groupConfig in testDashboardConfig.Groups)
      {
        foreach (var buildConfig in groupConfig.Builds)
        {
          BuildResult buildResult = TestDataGenerator.GetBuildResult();
          this.buildServiceMock.Setup(m => m.GetLastBuildStatus(It.IsAny<IConnectionParams>(), buildConfig.BuildConfigurationId, buildConfig.BranchName)).Returns(buildResult);
          buildResults.Add($"{groupConfig.Title}-{buildConfig.Title}-{buildConfig.BuildConfigurationId}-{buildConfig.BranchName}", buildResult);
        }
      }

      ActionResult<DashboardModel> result = this.controller.Get(testDashboardConfig.Slug);

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
        }
      }
    }

    [TestMethod]
    public void ShouldReturnBadRequestIfDashboardSlugIsEmpty()
    {
      string dashboardSlug = String.Empty;
      ActionResult result = this.controller.Get(dashboardSlug).Result;
      Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void ShouldReturnBadRequestIfDashboardSlugIsNull()
    {
      string dashboardSlug = null;
      ActionResult result = this.controller.Get(dashboardSlug).Result;
      Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void ShouldReturnNotFoundIfDashboardSlugIsUnknown()
    {
      string dashboardSlug = "NOT_EXISTS";
      ActionResult result = this.controller.Get(dashboardSlug).Result;
      Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }
  }
}
