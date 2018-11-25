namespace BuildMonitor.Infrastructure.TeamCity
{
  internal static class TypeLock<T>
  {
    public static object Lock { get; } = new object();
  }
}
