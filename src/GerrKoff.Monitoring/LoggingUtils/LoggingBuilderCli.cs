using Serilog;
using Serilog.Enrichers.Span;

namespace GerrKoff.Monitoring.LoggingUtils;

class LoggingBuilderCli : LoggingBuilder
{
    protected override void LoggerSetup(LoggerConfiguration configuration)
    {
        configuration
            .Enrich.WithSpan();
    }
}
