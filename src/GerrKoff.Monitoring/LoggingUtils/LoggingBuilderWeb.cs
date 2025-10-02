using System;
using Serilog;
using Serilog.Enrichers.Span;

namespace GerrKoff.Monitoring.LoggingUtils;

internal sealed class LoggingBuilderWeb(IServiceProvider services) : LoggingBuilder
{
    protected override void LoggerSetup(LoggerConfiguration configuration)
    {
        configuration
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithSpan()
            .Enrich.WithClientIp()
            .Enrich.WithRequestHeader("User-Agent");
    }
}
