using System;
using Serilog;
using Serilog.Enrichers.Span;

namespace GerrKoff.Monitoring.LoggingUtils;

class LoggingBuilderWeb : LoggingBuilder
{
    private readonly IServiceProvider _services;

    public LoggingBuilderWeb(IServiceProvider services)
    {
        _services = services;
    }

    protected override void LoggerSetup(LoggerConfiguration configuration)
    {
        configuration
            .ReadFrom.Services(_services)
            .Enrich.FromLogContext()
            .Enrich.WithSpan()
            .Enrich.WithClientIp()
            .Enrich.WithClientAgent();
    }
}
