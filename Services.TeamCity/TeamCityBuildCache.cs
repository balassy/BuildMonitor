using System;
using BuildMonitor.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace BuildMonitor.Services.TeamCity
{
  public class TeamCityBuildCache : ITeamCityBuildCache
  {
    private readonly IMemoryCache cache;

    public TeamCityBuildCache(IMemoryCache cache)
    {
      this.cache = cache ?? throw new ArgumentNullException(nameof(cache), "Please specify the memory cache for the TeamCity build cache!");
    }

    public TestResults GetOrAddTestResults(string buildId, Func<TestResults> factory)
    {
      if (String.IsNullOrEmpty(buildId))
      {
        throw new ArgumentNullException(nameof(buildId), "Please specify the ID of the build to get its test results from the build cache!");
      }

      if (factory == null)
      {
        throw new ArgumentNullException(nameof(factory), "Please specify the factory method to create new cache item!");
      }

      string cacheKey = $"TestResults-{buildId}";
      TestResults testResults = this.GetOrAdd(cacheKey, factory);
      return testResults;
    }

    private T GetOrAdd<T>(string cacheKey, Func<T> factory)
    {
      if (String.IsNullOrEmpty(cacheKey))
      {
        throw new ArgumentNullException(nameof(cacheKey), "Please specify the key of the entry to get from the cache!");
      }

      if (factory == null)
      {
        throw new ArgumentNullException(nameof(factory), "Please specify the factory method to create new cache item!");
      }

      // Not using IMemoryCache.GetOrCreate to avoid multiple calls of the factory method.
      // Read more in the Additional Notes on https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-2.1.
      // Solution from here: https://tpodolak.com/blog/2017/12/13/asp-net-core-memorycache-getorcreate-calls-factory-method-multiple-times/
      if (this.cache.TryGetValue(cacheKey, out T result))
      {
        return result;
      }

      lock (TypeLock<T>.Lock)
      {
        if (this.cache.TryGetValue(cacheKey, out result))
        {
          return result;
        }

        result = factory();
        TimeSpan slidingExpiration = new TimeSpan(24, 0, 0);
        this.cache.Set(cacheKey, result, slidingExpiration);

        return result;
      }
    }
  }
}
