using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus.DotNetRuntime;

namespace GerrKoff.Monitoring.MetricsUtils;

internal abstract class MetricsCollector(ILogger<MetricsCollector> logger, IOptions<MetricsOptions> options)
{
    private readonly MetricsOptions _options = options.Value;

    private IDisposable? _metrics;

    public bool IsEnabled => _options.MetricsConfig?.MetricsEnabled ?? false;

    protected void StartCollecting()
    {
        var builder = DotNetRuntimeStatsBuilder.Default();

#pragma warning disable CA1848 // Use LoggerMessage delegates for better performance
        builder.WithErrorHandler(ex => logger.LogError(ex, "Unexpected exception occurred in metrics collector"));
#pragma warning restore CA1848

        SetupLabels();

        _metrics = builder.StartCollecting();

        ExportVersionIfExist();

#pragma warning disable CA1848 // Use LoggerMessage delegates for better performance
        logger.LogDebug("Started metrics collector");
#pragma warning restore CA1848
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
            { Constants.LabelInstance, instance },
        });
    }

    private void ExportVersionIfExist()
    {
        if (_options.Version != null)
        {
            Prometheus.Metrics
                .CreateGauge("app_version", string.Empty, "app_version")
                .WithLabels(_options.Version)
                .Set(1);
        }
    }
}
