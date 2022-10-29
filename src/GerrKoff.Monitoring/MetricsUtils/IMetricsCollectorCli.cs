using System.Threading;
using System.Threading.Tasks;

namespace GerrKoff.Monitoring.MetricsUtils;

public interface IMetricsCollectorCli
{
    Task Collect(CancellationToken cancellationToken);
}
