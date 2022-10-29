namespace GerrKoff.Monitoring.MetricsUtils;

public class MetricsOptions
{
    public bool MetricsEnabled { get; init; } = false;
    public int? MetricsPort { get; init; } = null;
}
