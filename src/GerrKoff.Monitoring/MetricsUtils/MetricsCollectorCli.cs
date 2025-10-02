using System;
using System.Threading;
using System.Threading.Tasks;
using GerrKoff.Monitoring.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus;

namespace GerrKoff.Monitoring.MetricsUtils;

internal sealed class MetricsCollectorCli(ILogger<MetricsCollectorCli> logger, IOptions<MetricsOptions> options)
    : MetricsCollector(logger, options), IMetricsCollectorCli
{
    private readonly ILogger<MetricsCollectorCli> _logger = logger;
    private readonly MetricsOptions _options = options.Value;

    public Task Collect(CancellationToken cancellationToken)
    {
        if (!IsEnabled)
            return Task.CompletedTask;

        var server = RunMetricsServer();
        StartCollecting();

        var tcs = new TaskCompletionSource();

        cancellationToken.Register(() =>
        {
            StopCollecting();
            server.Dispose();
            tcs.SetResult();
        });

        return tcs.Task;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "MetricServer.Start() returns IDisposable that owns the server lifetime")]
    private IDisposable RunMetricsServer()
    {
        if (_options.MetricsConfig?.MetricsPort == null)
            throw new MonitoringException("Metrics port is required");

        var server = new MetricServer(_options.MetricsConfig.MetricsPort.Value);
        var runningServer = server.Start();

#pragma warning disable CA1848 // Use LoggerMessage delegates for better performance
        _logger.LogTrace("Metrics server on [{Port}]", _options.MetricsConfig.MetricsPort.Value);
#pragma warning restore CA1848
        return runningServer;
    }
}
