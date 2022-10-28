using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

namespace GerrKoff.Monitoring.LoggingUtils;

abstract class LoggingBuilder
{
    public void Build(
        LoggerConfiguration loggerConfiguration,
        IConfiguration appConfiguration,
        LoggingOptions options
    )
    {
        loggerConfiguration
            .ReadFrom.Configuration(appConfiguration)
            .WriteTo.Console();

        LoggerSetup(loggerConfiguration);

        var lokiOptions = options.LokiOptions ?? GetLokiOptionsFromSettings(appConfiguration);
        var url = lokiOptions?.LokiUrl;
        var credentials = GetLokiCredentialsFromOptions(lokiOptions);
        var app = options.App;
        var environment = options.Environment ?? "unknown";
        var instance = options.Instance ?? "unknown";

        if (!string.IsNullOrWhiteSpace(url))
        {
            Logging.Logger.Information("Loki is connected to [{Url}]", url);
            loggerConfiguration.WriteTo.GrafanaLoki(
                url,
                new List<LokiLabel>
                {
                    new() { Key = "app", Value = app },
                    new() { Key = "env", Value = environment },
                    new() { Key = "instance", Value = instance },
                },
                LokiLabelFiltrationMode.Include,
                new string[] { },
                textFormatter: new LokiJsonTextFormatter(),
                credentials: credentials
            );
        }

        Logging.Logger.Information("Application [{Application}] Environment [{Environment}] Instance [{Instance}]",
            app, environment, instance);
    }

    private LokiOptions? GetLokiOptionsFromSettings(IConfiguration appConfiguration)
    {
        return appConfiguration.GetSection(Constants.MonitoringSectionName).Get<LokiOptions?>();
    }

    private LokiCredentials? GetLokiCredentialsFromOptions(LokiOptions? options)
    {
        return string.IsNullOrEmpty(options?.LokiUser) || string.IsNullOrEmpty(options.LokiPass)
            ? null
            : new LokiCredentials { Login = options.LokiUser, Password = options.LokiPass };
    }

    protected abstract void LoggerSetup(LoggerConfiguration configuration);
}
