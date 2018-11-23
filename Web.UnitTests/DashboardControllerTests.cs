using System;
using BuildMonitor.Domain.Configuration;
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
    private Mock<IDashboardService> serviceMock;

    private DashboardController controller;

    [TestInitialize]
    public void InitializeTest()
    {
      this.serviceMock = new Mock<IDashboardService>();
      this.controller = new DashboardController(this.serviceMock.Object);
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

    [TestMethod]
    public void ShouldReturnDashboardDataFromTheService()
    {
      string dashboardSlug = TestDataGenerator.GetSlug();
      DashboardModel dashboardModel = new DashboardModel();

      DashboardConfig dashboardConfig = TestDataGenerator.GetDashboardConfig();
      this.serviceMock.Setup(m => m.GetDashboardConfig(dashboardSlug)).Returns(dashboardConfig);
      this.serviceMock.Setup(m => m.GetBuildResults(dashboardConfig)).Returns(dashboardModel);

      DashboardModel result = this.controller.Get(dashboardSlug).Value;

      Assert.AreEqual(dashboardModel, result);
    }
  }
}
