using System.Linq;
using BuildMonitor.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildMonitor.Web.UnitTests
{
  [TestClass]
  public class SampleDataControllerTests
  {
    [TestMethod]
    public void ShouldReturnWeatherForecastData()
    {
      SampleDataController controller = new SampleDataController();
      Assert.AreEqual(5, controller.WeatherForecasts().Count());
    }
  }
}
