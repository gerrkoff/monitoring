using GerrKoff.Monitoring;
using GerrKoff.Monitoring.MetricsUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

const string appName = "example-app";

await Logging.RunSafeAsync(async () =>
{
    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false)
        .Build();

    Logging.UseLoggingCli(config, new (appName));

    var services = new ServiceCollection()
        .AddLoggingCli()
        .AddMetricsCli(config, new (appName))
        .BuildServiceProvider();

    var metricsCollector = services.GetRequiredService<IMetricsCollectorCli>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    var cancelationToken = new CancellationTokenSource(10*1000).Token;

    var metricsCollectingTask = metricsCollector.Collect(cancelationToken);

    Console.WriteLine("Hello, World!");

    await metricsCollectingTask;

    logger.LogInformation("App finished");
}, () => "v1.0.0");
