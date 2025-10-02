using GerrKoff.Monitoring.Common;

namespace GerrKoff.Monitoring.LoggingUtils;

public class LoggingOptions(string app) : CommonOptions(app)
{
    public LoggingConfig? LoggingConfig { get; init; }
}
