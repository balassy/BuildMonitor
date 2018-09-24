using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuildMonitor.Web.Configuration
{
  public class ConnectionConfig
  {
    [Required]
    public string Host { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
  }
}
