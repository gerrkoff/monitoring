using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GerrKoff.Monitoring.MetricsUtils;

class MetricsCollectorWeb : MetricsCollector, IHostedService
{
    public MetricsCollectorWeb(ILogger<MetricsCollectorWeb> logger, IOptions<MetricsOptions> options) : base(logger, options)
    {
    }

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
