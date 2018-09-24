using System;
using System.Collections.Generic;
using System.Linq;
using BuildMonitor.Services.Interfaces;
using BuildMonitor.Web.Configuration;
using BuildMonitor.Web.Dashboard;
using BuildMonitor.Web.Dashboard.Models;
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

      this.buildServiceMock = new Mock<IBuildService>();
      this.buildServiceMock.Setup(m => m.GetLastBuildStatus(It.IsAny<string>(), It.IsAny<string>())).Returns(this.testBuildResult);
      this.controller = new DashboardController(this.buildServiceMock.Object, configServiceMock.Object);
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
          this.buildServiceMock.Setup(m => m.GetLastBuildStatus(buildConfig.BuildConfigurationId, buildConfig.BranchName)).Returns(buildResult);
          buildResults.Add($"{groupConfig.Title}-{buildConfig.Title}-{buildConfig.BuildConfigurationId}-{buildConfig.BranchName}", buildResult);
        }
      }

      ActionResult<DashboardResultModel> result = this.controller.Get(testDashboardConfig.Slug);

      DashboardResultModel dashboardModel = result.Value;
      Assert.AreEqual(testDashboardConfig.Title, dashboardModel.Title);
      for (int groupIndex = 0; groupIndex < dashboardModel.Groups.Count; groupIndex++)
      {
        GroupConfig groupConfig = testDashboardConfig.Groups[groupIndex];
        BuildResultGroupModel groupModel = dashboardModel.Groups[groupIndex];
        Assert.AreEqual(groupConfig.Title, groupModel.Title);

        for (int buildIndex = 0; buildIndex < groupModel.Builds.Count; buildIndex++)
        {
          BuildConfig buildConfig = groupConfig.Builds[buildIndex];
          BuildResultModel buildModel = groupModel.Builds[buildIndex];
          Assert.AreEqual(buildConfig.Title, buildModel.Title);
          Assert.AreEqual(buildConfig.BranchName, buildModel.BranchName);

          BuildResult buildResult = buildResults[$"{groupConfig.Title}-{buildConfig.Title}-{buildConfig.BuildConfigurationId}-{buildConfig.BranchName}"];
          Assert.AreEqual(buildResult.Status, buildModel.Status);
          Assert.AreEqual(buildResult.BuildId, buildModel.BuildId);
          Assert.AreEqual(buildResult.CompletedTimestamp, buildModel.CompletedTimestamp);
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
