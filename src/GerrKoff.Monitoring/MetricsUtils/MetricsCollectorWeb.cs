using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GerrKoff.Monitoring.MetricsUtils;

class MetricsCollectorWeb : MetricsCollector, IHostedService
{
    private readonly MetricsOptions _options;

    public MetricsCollectorWeb(ILogger<MetricsCollectorWeb> logger, IOptions<MetricsOptions> options) : base(logger)
    {
        _options = options.Value;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_options.MetricsConfig?.MetricsEnabled ?? false)
            StartCollecting(_options);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        StopCollecting();
        return Task.CompletedTask;
    }
}
