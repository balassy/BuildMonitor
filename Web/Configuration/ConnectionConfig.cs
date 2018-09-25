using System.ComponentModel.DataAnnotations;
using BuildMonitor.Services.Interfaces;

namespace BuildMonitor.Web.Configuration
{
  public class ConnectionConfig : IConnectionParams
  {
    [Required]
    public string Host { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
  }
}
