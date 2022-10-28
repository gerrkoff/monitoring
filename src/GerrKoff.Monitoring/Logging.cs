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

public static class Logging
{
    public static ILogger Logger => Log.Logger;

    public static async void RunSafe(Action run, Func<string?> getVersion)
    {
        await RunSafeAsync(() =>
        {
            run();
            return Task.CompletedTask;
        }, getVersion);
    }

    public static async Task RunSafeAsync(Func<Task> run, Func<string?> getVersion)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .WriteTo.Console()
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

    public static void UseLogging(IConfiguration appConfiguration, LoggingOptions options)
    {
        var loggerConfiguration = new LoggerConfiguration();

        new LoggingBuilderCli().Build(loggerConfiguration, appConfiguration, options);

        Log.Logger = loggerConfiguration.CreateLogger();
    }

    public static IHostBuilder UseLogging(this IHostBuilder hostBuilder, LoggingOptions options) =>
        hostBuilder.UseSerilog(
            (context, services, configuration) =>
                new LoggingBuilderWeb(services).Build(configuration, context.Configuration, options)
        );

    public static void UseRequestLogging(this IApplicationBuilder app) => app.UseSerilogRequestLogging();

    public static void AddLoggingServices(this IServiceCollection services, IConfiguration configuration) => services.AddHttpContextAccessor();
}
