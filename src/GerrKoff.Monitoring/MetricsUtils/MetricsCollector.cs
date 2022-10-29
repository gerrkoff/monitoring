using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Prometheus;
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

    protected void StartCollecting(MetricsOptions options)
    {
        var builder = DotNetRuntimeStatsBuilder.Default();

        builder.WithErrorHandler(ex => _logger.LogError(ex, "Unexpected exception occurred in metrics collector"));

        var app = options.App;
        var environment = options.Environment ?? Constants.NoValue;
        var instance = options.Instance ?? Constants.NoValue;

        var collectorRegistry = new CollectorRegistry();
        collectorRegistry.SetStaticLabels(new Dictionary<string, string>
        {
            { Constants.LabelApp, app },
            { Constants.LabelEnvinronment, environment },
            { Constants.LabelInstance, instance }
        });

        _metrics = builder.StartCollecting(collectorRegistry);

        _logger.LogDebug("Started metrics collector");
    }

    protected void StopCollecting()
    {
        _metrics?.Dispose();
    }
}
