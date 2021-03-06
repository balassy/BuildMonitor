﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using BuildMonitor.Domain.Entities;
using BuildMonitor.Infrastructure.TeamCity;
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
        Assert.ThrowsException<ArgumentNullException>(() => this.cache.GetOrAddTestResult(null, null));
      }

      [TestMethod]
      public void ShouldThrowIfFactoryMethodIsNotSpecified()
      {
        string buildId = TestDataGenerator.GetBuildId();
        Assert.ThrowsException<ArgumentNullException>(() => this.cache.GetOrAddTestResult(buildId, null));
      }

      [TestMethod]
      public void ShouldCallFactoryMethodOnlyOnce()
      {
        // Intentionally not mocking the IMemoryCache dependency.
        Mock<IOptions<MemoryCacheOptions>> optionsMock = new Mock<IOptions<MemoryCacheOptions>>();
        optionsMock.SetupGet(m => m.Value).Returns(new MemoryCacheOptions());
        TeamCityBuildCache cache = new TeamCityBuildCache(new MemoryCache(optionsMock.Object));

        Mock<Func<TestRunResult>> factoryMock = new Mock<Func<TestRunResult>>();
        factoryMock.Setup(f => f()).Returns(new TestRunResult());

        string buildId = TestDataGenerator.GetBuildId();
        List<Thread> threads = Enumerable.Range(0, 10).Select(_ => new Thread(() => cache.GetOrAddTestResult(buildId, factoryMock.Object))).ToList();
        threads.ForEach(thread => thread.Start());
        threads.ForEach(thread => thread.Join());

        factoryMock.Verify(f => f(), Times.Once());
      }
    }
  }
}
