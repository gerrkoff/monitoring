using System;
using System.Threading;
using System.Threading.Tasks;
using GerrKoff.Monitoring.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus;

namespace GerrKoff.Monitoring.MetricsUtils;

class MetricsCollectorCli : MetricsCollector, IMetricsCollectorCli
{
    private readonly ILogger<MetricsCollectorCli> _logger;
    private readonly MetricsOptions _options;

    public MetricsCollectorCli(ILogger<MetricsCollectorCli> logger, IOptions<MetricsOptions> options) : base(logger, options)
    {
        _logger = logger;
        _options = options.Value;
    }

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

    private IDisposable RunMetricsServer()
    {
        if (_options.MetricsConfig?.MetricsPort == null)
            throw new MonitoringException("Metrics port is required");

        var server = new MetricServer(_options.MetricsConfig.MetricsPort.Value).Start();

        _logger.LogTrace("Metrics server on [{Port}]", _options.MetricsConfig.MetricsPort.Value);
        return server;
    }
}
