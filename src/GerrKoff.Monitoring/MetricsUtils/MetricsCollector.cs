using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus.DotNetRuntime;

namespace GerrKoff.Monitoring.MetricsUtils;

abstract class MetricsCollector
{
    private readonly ILogger<MetricsCollector> _logger;
    private readonly MetricsOptions _options;

    private IDisposable? _metrics;

    protected MetricsCollector(ILogger<MetricsCollector> logger, IOptions<MetricsOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public bool IsEnabled => _options.MetricsConfig?.MetricsEnabled ?? false;

    protected void StartCollecting()
    {
        var builder = DotNetRuntimeStatsBuilder.Default();

        builder.WithErrorHandler(ex => _logger.LogError(ex, "Unexpected exception occurred in metrics collector"));

        SetupLabels();

        _metrics = builder.StartCollecting();

        _logger.LogDebug("Started metrics collector");
    }

    protected void StopCollecting()
    {
        _metrics?.Dispose();
    }

    private void SetupLabels()
    {
        var app = _options.App;
        var environment = _options.Environment ?? Constants.NoValue;
        var instance = _options.Instance ?? Constants.NoValue;

        Prometheus.Metrics.DefaultRegistry.SetStaticLabels(new Dictionary<string, string>
        {
            { Constants.LabelApp, app },
            { Constants.LabelEnvinronment, environment },
            { Constants.LabelInstance, instance }
        });
    }
}
