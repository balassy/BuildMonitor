using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace BuildMonitor.Web
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      Program.CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
      return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
  }
}
