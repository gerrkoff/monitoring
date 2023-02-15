using GerrKoff.Monitoring;
using GerrKoff.Monitoring.LoggingUtils;
using GerrKoff.Monitoring.MetricsUtils;

const string appName = "example-app";

Logging.RunSafe(() =>
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseLoggingWeb(new LoggingOptions(appName));

    builder.Services.AddLoggingWeb();
    builder.Services.AddMetricsWeb(builder.Configuration, new MetricsOptions(appName));

    var app = builder.Build();

    app.UseRequestLogging();
    app.UseMetrics();

    app.MapGet("/", context => context.Response.WriteAsync("Hello World!"));
    app.Run();
}, () => "v1.0.0");
