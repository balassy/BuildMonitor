using System;

namespace BuildMonitor.Domain.ValueObjects
{
  public class BuildServerConnection
  {
    public BuildServerConnection(string host, string username, string password)
    {
      this.Host = host ?? throw new ArgumentNullException(nameof(host), "Please specify the host for the build server connection!");
      this.Username = username;
      this.Password = password;
    }

    public string Host { get; private set; }

    public string Username { get; private set; }

    public string Password { get; private set; }
  }
}
