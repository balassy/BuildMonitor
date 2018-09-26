using System.ComponentModel.DataAnnotations;
using BuildMonitor.Services.Interfaces;

namespace BuildMonitor.Web.Configuration
{
  public class ConnectionConfig : IConnectionParams
  {
    [Required]
    public string Host { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }
  }
}
