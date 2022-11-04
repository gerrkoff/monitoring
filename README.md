# GerrKoff.Monitoring

## Setup Web
[full example](https://github.com/gerrkoff/monitoring/blob/main/src/Example.Web/Program.cs)

Wrap program
```csharp
Logging.RunSafe(() =>
{
    // ...
}, () => "v1.0.0");
```
Setup logging
```csharp
builder.Host.UseLoggingWeb(new ("example-app"));
```
Setup metrics
```csharp
builder.Services.AddMetricsWeb(builder.Configuration, new ("example-app"));
```
Add request info logging
```csharp
app.UseRequestLogging();
```
Add metrics export endpoint
```csharp
app.UseMetrics();
```
appsettings.json
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.AspNetCore": "Warning"
      }
    }
  },
  "Monitoring": {
    "LokiUrl": "",
    "LokiUser": "",
    "LokiPass": "",
    "MetricsEnabled": true
  }
}
```

## Setup CLI
[full example](https://github.com/gerrkoff/monitoring/blob/main/src/Example.Cli/Program.cs)

Wrap program
```csharp
await Logging.RunSafeAsync(async () =>
{
    // ...
}, () => "v1.0.0");
```
Initialize logging
```csharp
Logging.UseLoggingCli(config, new ("example-app"));
```
Register logging and metrics services
```csharp
services
    .AddLoggingCli()
    .AddMetricsCli(config, new ("example-app"));
```
Start metrics collector
```csharp
var metricsCollectingTask = services
    .GetRequiredService<IMetricsCollectorCli>()
    .Collect(cancelationToken);
```
appsettings.json
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.AspNetCore": "Warning"
      }
    }
  },
  "Monitoring": {
    "LokiUrl": "",
    "LokiUser": "",
    "LokiPass": "",
    "MetricsEnabled": true,
    "MetricsPort": 3000
  }
}
```

## Grafana Dashboards
[App Metrics](https://grafana.com/grafana/dashboards/17327-app-metrics/)

[App Logs](https://grafana.com/grafana/dashboards/17328-app-logs/)

[Overview](https://grafana.com/grafana/dashboards/17329-overview/)
