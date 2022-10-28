namespace GerrKoff.Monitoring.LoggingUtils;

public class LoggingOptions
{
    public LoggingOptions(string app)
    {
        App = app;
    }

    public string App { get; }
    public string? Environment { get; init; }
    public string? Instance { get; init; }
    public LokiOptions? LokiOptions { get; init; }
}
