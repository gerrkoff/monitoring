using GerrKoff.Monitoring;

const string appName = "example-app";

Logging.RunSafe(() =>
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseLoggingWeb(new (appName));

    builder.Services.AddMetricsWeb(builder.Configuration, new (appName));

    var app = builder.Build();

    app.UseRequestLogging();
    app.UseMetrics();

    app.MapGet("/", context => context.Response.WriteAsync("Hello World!"));
    app.Run();
}, () => "v1.0.0");
