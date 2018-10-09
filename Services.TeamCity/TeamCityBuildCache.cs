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

    public TestResults GetOrAdd(string buildId, Func<TestResults> factory)
    {
      if (String.IsNullOrEmpty(buildId))
      {
        throw new ArgumentNullException(nameof(buildId), "Please specify the ID of the build to get from the build cache!");
      }

      if (factory == null)
      {
        throw new ArgumentNullException(nameof(factory), "Please specify the factory method to create new cache item!");
      }

      // Not using IMemoryCache.GetOrCreate to avoid multiple calls of the factory method.
      // Read more in the Additional Notes on https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-2.1.
      // Solution from here: https://tpodolak.com/blog/2017/12/13/asp-net-core-memorycache-getorcreate-calls-factory-method-multiple-times/
      if (this.cache.TryGetValue(buildId, out TestResults result))
      {
        return result;
      }

      lock (TypeLock<TestResults>.Lock)
      {
        if (this.cache.TryGetValue(buildId, out result))
        {
          return result;
        }

        result = factory();
        TimeSpan slidingExpiration = new TimeSpan(24, 0, 0);
        this.cache.Set(buildId, result, slidingExpiration);

        return result;
      }
    }
  }
}
