namespace GerrKoff.Monitoring.Common;

public class CommonOptions
{
    public CommonOptions(string app)
    {
        App = app;
    }

    public string App { get; }
    public string? Environment { get; init; }
    public string? Instance { get; init; }
}
