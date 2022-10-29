using GerrKoff.Monitoring.MetricsUtils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prometheus;

namespace GerrKoff.Monitoring;

public static class Metrics
{
    private static void AddMetricsCore(this IServiceCollection services, IConfiguration configuration, MetricsOptions options)
    {
        var metricsConfig = options.MetricsConfig;
        if (metricsConfig == null)
        {
            metricsConfig = new MetricsConfig();
            configuration.GetSection(Constants.MonitoringSectionName).Bind(metricsConfig);
        }

        services.AddSingleton<IOptions<MetricsOptions>>(_ => new OptionsWrapper<MetricsOptions>(
            new MetricsOptions(options.App)
            {
                Environment = options.Environment,
                Instance = options.Instance,
                MetricsConfig = metricsConfig,
            }));
    }

    public static void AddMetricsWeb(this IServiceCollection services, IConfiguration configuration, MetricsOptions options)
    {
        services.AddMetricsCore(configuration, options);
        services.AddSingleton<MetricsCollectorWeb>();
        services.AddHostedService(s => s.GetRequiredService<MetricsCollectorWeb>());
    }

    public static void AddMetricsCli(this IServiceCollection services, IConfiguration configuration, MetricsOptions options)
    {
        services.AddMetricsCore(configuration, options);
        services.AddSingleton<IMetricsCollectorCli, MetricsCollectorCli>();
    }

    public static void UseMetrics(this IApplicationBuilder app)
    {
        if (!app.ApplicationServices.GetRequiredService<MetricsCollectorWeb>().IsEnabled) return;

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
