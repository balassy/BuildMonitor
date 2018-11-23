namespace BuildMonitor.Domain.Entities
{
  public class TestRunResult
  {
    /// <summary>
    /// Gets or sets the number of successfully passed test cases.
    /// </summary>
    public int PassedCount { get; set; }

    /// <summary>
    /// Gets or sets the number of test cases that were failed.
    /// </summary>
    public int FailedCount { get; set; }

    /// <summary>
    /// Gets or sets the number of test cases that were ignored and not run.
    /// </summary>
    public int IgnoredCount { get; set; }
  }
}
