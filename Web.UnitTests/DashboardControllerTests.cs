using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildMonitor.Services.Interfaces;
using BuildMonitor.Web.Dashboard;
using BuildMonitor.Web.Dashboard.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BuildMonitor.Web.UnitTests
{
  [TestClass]
  public class DashboardControllerTests
  {
    private DashboardController controller;

    private BuildResult testBuildResult;

    [TestInitialize]
    public void InitializeTest()
    {
      this.testBuildResult = TestDataGenerator.GetBuildResult();

      var buildServiceMock = new Mock<IBuildService>();
      buildServiceMock.Setup(m => m.GetLastBuildStatus(It.IsAny<string>(), It.IsAny<string>())).Returns(this.testBuildResult);

      this.controller = new DashboardController(buildServiceMock.Object);
    }

    [TestMethod]
    public void ShouldReturnDashboardResultFromTheService()
    {
      string dashboardId = TestDataGenerator.GetDashboardId();
      DashboardResultModel returnedResult = this.controller.Get(dashboardId);
      BuildResultModel returnedBuildResult = returnedResult.Groups[0].Builds[0];
      Assert.AreEqual(this.testBuildResult.BranchName, returnedBuildResult.BranchName);
      Assert.AreEqual(this.testBuildResult.Status, returnedBuildResult.Status);
      Assert.AreEqual(this.testBuildResult.TriggeredBy, returnedBuildResult.TriggeredBy);
    }
  }
}
