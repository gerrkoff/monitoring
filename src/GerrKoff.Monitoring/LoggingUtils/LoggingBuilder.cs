using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

namespace GerrKoff.Monitoring.LoggingUtils;

internal abstract class LoggingBuilder
{
    public void Build(
        LoggerConfiguration loggerConfiguration,
        IConfiguration appConfiguration,
        LoggingOptions options)
    {
        loggerConfiguration
            .ReadFrom.Configuration(appConfiguration)
            .WriteTo.Console(formatProvider: System.Globalization.CultureInfo.InvariantCulture);

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
                [
                    new() { Key = Constants.LabelApp, Value = app },
                    new() { Key = Constants.LabelEnvinronment, Value = environment },
                    new() { Key = Constants.LabelInstance, Value = instance },
                ],
                LokiLabelFiltrationMode.Include,
                [],
                textFormatter: new LokiJsonTextFormatter(),
                credentials: credentials);
        }

        Logging.Logger.Information(
            "Application [{Application}] Environment [{Environment}] Instance [{Instance}]",
            app,
            environment,
            instance);
    }

    protected abstract void LoggerSetup(LoggerConfiguration configuration);

    private static LoggingConfig? GetLoggingConfigFromSettings(IConfiguration appConfiguration)
    {
        return appConfiguration.GetSection(Constants.MonitoringSectionName).Get<LoggingConfig?>();
    }

    private static LokiCredentials? GetLokiCredentialsFromConfig(LoggingConfig? options)
    {
        return string.IsNullOrEmpty(options?.LokiUser) || string.IsNullOrEmpty(options.LokiPass)
            ? null
            : new LokiCredentials { Login = options.LokiUser, Password = options.LokiPass };
    }
}
