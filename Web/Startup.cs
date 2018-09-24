using BuildMonitor.Services.Interfaces;
using BuildMonitor.Services.TeamCity;
using BuildMonitor.Web.Configuration;
using BuildMonitor.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace BuildMonitor.Web
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      this.Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

#pragma warning disable CA1822 // MarkMembersAsStatic

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services
        .AddMvc()
        .AddJsonOptions(options =>
        {
          options.SerializerSettings.Formatting = Formatting.Indented;
        })
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

      // In production, the Angular files will be served from this directory
      services.AddSpaStaticFiles(configuration =>
      {
        configuration.RootPath = "ClientApp/dist";
      });

      services.Configure<AppConfig>(this.Configuration.GetSection("AppConfig"));

      // Set up Dependency Injection.
      services.AddSingleton<IBuildService, TeamCityBuildService>();
      services.AddTransient<IAppConfigService, AppConfigService>();
      services.AddSingleton<ITimestampConverter, HumanizerTimestampConverter>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseSpaStaticFiles();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                 name: "default",
                 template: "{controller}/{action=Index}/{id?}");
      });

      app.UseSpa(spa =>
      {
        // To learn more about options for serving an Angular SPA from ASP.NET Core,
        // see https://go.microsoft.com/fwlink/?linkid=864501
        spa.Options.SourcePath = "ClientApp";

        if (env.IsDevelopment())
        {
          spa.UseAngularCliServer(npmScript: "start");
        }
      });
    }

#pragma warning restore CA1822 // MarkMembersAsStatic
  }
}
