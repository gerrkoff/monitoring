using GerrKoff.Monitoring.Common;

namespace GerrKoff.Monitoring.LoggingUtils;

public class LoggingOptions : CommonOptions
{
    public LoggingOptions(string app) : base(app)
    {
    }

    public LoggingConfig? LoggingConfig { get; init; }
}
