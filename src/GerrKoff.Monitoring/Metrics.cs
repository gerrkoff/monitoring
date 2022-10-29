using GerrKoff.Monitoring.MetricsUtils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace GerrKoff.Monitoring;

public static class Metrics
{
    private static void AddMetricsCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MetricsOptions>(options => configuration.GetSection(Constants.MonitoringSectionName).Bind(options));
    }

    public static void AddMetricsWeb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMetricsCore(configuration);
        services.AddHostedService<MetricsCollectorWeb>();
    }

    public static void AddMetricsCli(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMetricsCore(configuration);
        services.AddSingleton<IMetricsCollectorCli, MetricsCollectorCli>();
    }

    public static void UseMetrics(this IApplicationBuilder app)
    {
        app.UseHttpMetrics();
        app.UseMetricServer();
    }

    // public static void UseMetricsServer(this IApplicationBuilder app)
    // {
    //
    // }

    // public static void MapMetricsEndpoint(this IEndpointRouteBuilder endpoints)
    // {
    //     endpoints.MapMetrics();
    // }
}
