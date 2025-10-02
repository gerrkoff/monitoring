using System.Diagnostics.CodeAnalysis;

namespace GerrKoff.Monitoring.LoggingUtils;

public class LoggingConfig
{
    [SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "Configuration class")]
    public string LokiUrl { get; init; } = string.Empty;

    public string LokiUser { get; init; } = string.Empty;

    public string LokiPass { get; init; } = string.Empty;
}
