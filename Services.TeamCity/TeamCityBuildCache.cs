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

      // TODO: https://tpodolak.com/blog/2017/12/13/asp-net-core-memorycache-getorcreate-calls-factory-method-multiple-times/
      return this.cache.GetOrCreate(buildId, cacheEntry =>
      {
        cacheEntry.SetSlidingExpiration(new TimeSpan(24, 0, 0));
        return factory();
      });
    }
  }
}
