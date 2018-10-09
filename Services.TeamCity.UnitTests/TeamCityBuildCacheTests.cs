using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using BuildMonitor.Services.Interfaces;
using BuildMonitor.Services.TeamCity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BuildMonitor.Services.TeamCity.UnitTests
{
  [TestClass]
  public class TeamCityBuildCacheTests
  {
    private Mock<IMemoryCache> memoryCacheMock;
    private TeamCityBuildCache cache;

    [TestInitialize]
    public void InitializeTest()
    {
      this.memoryCacheMock = new Mock<IMemoryCache>();
      this.cache = new TeamCityBuildCache(this.memoryCacheMock.Object);
    }

    [TestClass]
    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Pattern to structure tests.")]
    public class ConstructorMethod : TeamCityBuildCacheTests
    {
      [TestMethod]
      public void ShouldThrowIfCacheIsNotSpecified()
      {
        Assert.ThrowsException<ArgumentNullException>(() => new TeamCityBuildCache(null));
      }
    }

    [TestClass]
    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Pattern to structure tests.")]
    public class GetOrAddMethod : TeamCityBuildCacheTests
    {
      [TestMethod]
      public void ShouldThrowIfBuildIdIsNotSpecified()
      {
        Assert.ThrowsException<ArgumentNullException>(() => this.cache.GetOrAddTestResults(null, null));
      }

      [TestMethod]
      public void ShouldThrowIfFactoryMethodIsNotSpecified()
      {
        string buildId = TestDataGenerator.GetBuildId();
        Assert.ThrowsException<ArgumentNullException>(() => this.cache.GetOrAddTestResults(buildId, null));
      }

      [TestMethod]
      public void ShouldCallFactoryMethodOnlyOnce()
      {
        // Intentionally not mocking the IMemoryCache dependency.
        Mock<IOptions<MemoryCacheOptions>> optionsMock = new Mock<IOptions<MemoryCacheOptions>>();
        optionsMock.SetupGet(m => m.Value).Returns(new MemoryCacheOptions());
        TeamCityBuildCache cache = new TeamCityBuildCache(new MemoryCache(optionsMock.Object));

        Mock<Func<TestResults>> factoryMock = new Mock<Func<TestResults>>();
        factoryMock.Setup(f => f()).Returns(new TestResults());

        string buildId = TestDataGenerator.GetBuildId();
        List<Thread> threads = Enumerable.Range(0, 10).Select(_ => new Thread(() => cache.GetOrAddTestResults(buildId, factoryMock.Object))).ToList();
        threads.ForEach(thread => thread.Start());
        threads.ForEach(thread => thread.Join());

        factoryMock.Verify(f => f(), Times.Once());
      }
    }
  }
}
