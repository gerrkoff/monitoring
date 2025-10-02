namespace GerrKoff.Monitoring.Common;

public class CommonOptions(string app)
{
    public string App { get; } = app;

    public string? Environment { get; init; }

    public string? Instance { get; init; }

    public string? Version { get; init; }
}
