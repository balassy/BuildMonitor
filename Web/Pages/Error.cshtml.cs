using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BuildMonitor.Web.Pages
{
  #pragma warning disable CA1716 // IdentifiersShouldNotMatchKeywords
  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public class Error : PageModel
  {
    public string RequestId { get; set; }

    public bool ShowRequestId => !String.IsNullOrEmpty(this.RequestId);

    public void OnGet()
    {
      this.RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier;
    }
  }
  #pragma warning restore CA1716 // IdentifiersShouldNotMatchKeywords
}
