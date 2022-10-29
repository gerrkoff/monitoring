namespace GerrKoff.Monitoring.MetricsUtils;

public class MetricsConfig
{
    public bool MetricsEnabled { get; init; } = false;
    public int? MetricsPort { get; init; } = null;
}
