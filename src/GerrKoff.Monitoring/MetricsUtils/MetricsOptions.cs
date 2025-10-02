using GerrKoff.Monitoring.Common;

namespace GerrKoff.Monitoring.MetricsUtils;

public class MetricsOptions(string app) : CommonOptions(app)
{
    public MetricsConfig? MetricsConfig { get; init; }
}
