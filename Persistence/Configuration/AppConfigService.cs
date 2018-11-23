using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BuildMonitor.Application.Interfaces;
using BuildMonitor.Domain.Configuration;
using Microsoft.Extensions.Options;

namespace BuildMonitor.Persistence.Configuration
{
  public class AppConfigService : IAppConfigService
  {
    private readonly IOptionsSnapshot<AppConfig> config;

    private readonly IOptionsSnapshot<ConnectionConfig> connectionConfig;

    public AppConfigService(IOptionsSnapshot<AppConfig> config, IOptionsSnapshot<ConnectionConfig> connectionConfig)
    {
      this.config = config ?? throw new ArgumentNullException(nameof(config), "Please specify the runtime config for the AppConfigService!");
      this.connectionConfig = connectionConfig ?? throw new ArgumentNullException(nameof(connectionConfig), "Please specify the connection configuration for the AppConfigService!");

      this.ValidateConnection();
    }

    public IReadOnlyList<DashboardConfig> Dashboards => this.config.Value.Dashboards.AsReadOnly();

    public ConnectionConfig Connection => this.connectionConfig.Value;

    private void ValidateConnection()
    {
      var results = new List<ValidationResult>();
      Validator.TryValidateObject(this.Connection, new ValidationContext(this.Connection), results, validateAllProperties: true);
      foreach (ValidationResult result in results)
      {
        throw new InvalidOperationException($"The connection configuration is invalid: {result.ErrorMessage} Please check the environment variables!");
      }
    }
  }
}
