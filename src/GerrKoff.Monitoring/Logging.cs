using System;
using System.Threading.Tasks;
using GerrKoff.Monitoring.LoggingUtils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace GerrKoff.Monitoring;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1724:Type names should not match namespaces", Justification = "This is a public API class name that cannot be changed without breaking changes")]
public static class Logging
{
    public static ILogger Logger => Log.Logger;

    public static async void RunSafe(Action run, Func<string?> getVersion)
    {
        await RunSafeAsync(
            () =>
            {
                run();
                return Task.CompletedTask;
            },
            getVersion);
    }

    public static async Task RunSafeAsync(Func<Task> run, Func<string?> getVersion)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .WriteTo.Console(formatProvider: System.Globalization.CultureInfo.InvariantCulture)
            .CreateBootstrapLogger();

        try
        {
            var version = getVersion() ?? "unknown";
            Log.Information("Starting app, version [{V}]", version);
            await run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "App terminated unexpectedly");
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static void UseLoggingCli(IConfiguration appConfiguration, AppMeta meta)
    {
        UseLoggingCli(
            appConfiguration,
            new LoggingOptions(meta.App)
            {
                Environment = meta.Environment,
                Instance = meta.Instance,
                Version = meta.Version(),
            });
    }

    public static void UseLoggingCli(IConfiguration appConfiguration, LoggingOptions options)
    {
        var loggerConfiguration = new LoggerConfiguration();

        new LoggingBuilderCli().Build(loggerConfiguration, appConfiguration, options);

        Log.Logger = loggerConfiguration.CreateLogger();
    }

    public static IHostBuilder UseLoggingWeb(this IHostBuilder hostBuilder, AppMeta meta)
    {
        return hostBuilder.UseLoggingWeb(new LoggingOptions(meta.App)
        {
            Environment = meta.Environment,
            Instance = meta.Instance,
            Version = meta.Version(),
        });
    }

    public static IServiceCollection AddLoggingWeb(this IServiceCollection services)
    {
        return services.AddHttpContextAccessor();
    }

    public static IHostBuilder UseLoggingWeb(this IHostBuilder hostBuilder, LoggingOptions options)
    {
        return hostBuilder.UseSerilog(
            (context, services, configuration) =>
                new LoggingBuilderWeb(services).Build(configuration, context.Configuration, options));
    }

    public static void UseRequestLogging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
    }

    public static IServiceCollection AddLoggingCli(this IServiceCollection services)
    {
        return services.AddLogging(b => b.AddSerilog());
    }
}
