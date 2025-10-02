using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GerrKoff.Monitoring.MetricsUtils;

internal sealed class MetricsCollectorWeb(ILogger<MetricsCollectorWeb> logger, IOptions<MetricsOptions> options)
    : MetricsCollector(logger, options), IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (IsEnabled)
            StartCollecting();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        StopCollecting();
        return Task.CompletedTask;
    }
}
