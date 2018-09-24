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

    [TestInitialize]
    public void InitializeTest()
    {
      this.testBuildResult = TestDataGenerator.GetBuildResult();
      this.testDashboardConfigs = TestDataGenerator.GetDashboardConfigs();

      var buildServiceMock = new Mock<IBuildService>();
      var configServiceMock = new Mock<IAppConfigService>();
      buildServiceMock.Setup(m => m.GetLastBuildStatus(It.IsAny<string>(), It.IsAny<string>())).Returns(this.testBuildResult);
      configServiceMock.Setup(m => m.Dashboards).Returns(this.testDashboardConfigs);

      this.controller = new DashboardController(buildServiceMock.Object, configServiceMock.Object);
    }

    [TestMethod]
    public void ShouldReturnDashboardResultFromTheService()
    {
      string dashboardSlug = this.testDashboardConfigs[1].Slug;
      DashboardResultModel returnedResult = this.controller.Get(dashboardSlug).Value;
      BuildResultModel returnedBuildResult = returnedResult.Groups[0].Builds[0];
      Assert.AreEqual(this.testBuildResult.BranchName, returnedBuildResult.BranchName);
      Assert.AreEqual(this.testBuildResult.Status, returnedBuildResult.Status);
      Assert.AreEqual(this.testBuildResult.TriggeredBy, returnedBuildResult.TriggeredBy);
    }

    [TestMethod]
    public void ShouldReturnNotFoundForUnknownDashboardSlug()
    {
      string dashboardSlug = "NOT_EXISTS";
      ActionResult result = this.controller.Get(dashboardSlug).Result;
      Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }
  }
}
