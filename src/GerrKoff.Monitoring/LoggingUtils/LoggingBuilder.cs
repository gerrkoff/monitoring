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

        var loggingConfig = options.LoggingConfig ?? GetLoggingConfigFromSettings(appConfiguration);
        var url = loggingConfig?.LokiUrl;
        var credentials = GetLokiCredentialsFromConfig(loggingConfig);
        var app = options.App;
        var environment = options.Environment ?? Constants.NoValue;
        var instance = options.Instance ?? Constants.NoValue;

        if (!string.IsNullOrWhiteSpace(url))
        {
            Logging.Logger.Information("Loki is connected to [{Url}]", url);
            loggerConfiguration.WriteTo.GrafanaLoki(
                url,
                new List<LokiLabel>
                {
                    new() { Key = Constants.LabelApp, Value = app },
                    new() { Key = Constants.LabelEnvinronment, Value = environment },
                    new() { Key = Constants.LabelInstance, Value = instance },
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

    private LoggingConfig? GetLoggingConfigFromSettings(IConfiguration appConfiguration)
    {
        return appConfiguration.GetSection(Constants.MonitoringSectionName).Get<LoggingConfig?>();
    }

    private LokiCredentials? GetLokiCredentialsFromConfig(LoggingConfig? options)
    {
        return string.IsNullOrEmpty(options?.LokiUser) || string.IsNullOrEmpty(options.LokiPass)
            ? null
            : new LokiCredentials { Login = options.LokiUser, Password = options.LokiPass };
    }

    protected abstract void LoggerSetup(LoggerConfiguration configuration);
}
