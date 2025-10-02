using System;
using System.Reflection;

namespace GerrKoff.Monitoring;

public class AppMeta(Type mainAssemblyType, string app, string? environment = null, string? instance = null)
{
    public string App { get; } = app;

    public string? Environment { get; } = environment;

    public string? Instance { get; } = instance;

    public static AppMeta FromEnvironment(Type mainAssemblyType, string app)
    {
        return new(mainAssemblyType, app, EnvFromEnv(), InstFromEnv());
    }

    public string Version()
    {
        return mainAssemblyType.Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? Constants.NoValue;
    }

    public static string? EnvFromEnv()
    {
        return System.Environment.GetEnvironmentVariable(Constants.EnvVarForEnvironment);
    }

    public static string? InstFromEnv()
    {
        return System.Environment.GetEnvironmentVariable(Constants.EnvVarForInstance);
    }
}
