using System;
using Microsoft.Extensions.Logging;
using Prometheus.DotNetRuntime;

namespace GerrKoff.Monitoring.MetricsUtils;

abstract class MetricsCollector
{
    private readonly ILogger<MetricsCollector> _logger;

    private IDisposable? _metrics;

    protected MetricsCollector(ILogger<MetricsCollector> logger)
    {
        _logger = logger;
    }

    protected void StartCollecting()
    {
        var builder = DotNetRuntimeStatsBuilder.Default();

        builder.WithErrorHandler(ex => _logger.LogError(ex, "Unexpected exception occurred in metrics collector"));

        _metrics = builder.StartCollecting();

        _logger.LogDebug("Started metrics collector");
    }

    protected void StopCollecting()
    {
        _metrics?.Dispose();
    }
}
