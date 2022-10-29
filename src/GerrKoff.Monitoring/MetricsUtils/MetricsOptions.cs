using GerrKoff.Monitoring.Common;

namespace GerrKoff.Monitoring.MetricsUtils;

public class MetricsOptions : CommonOptions
{
    public MetricsOptions(string app) : base(app)
    {
    }

    public MetricsConfig? MetricsConfig { get; init; }
}
