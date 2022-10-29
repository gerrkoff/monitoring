using System;
using System.Reflection;

namespace GerrKoff.Monitoring;

public class AppMeta
{
    private readonly Type _mainAssemblyType;
    private readonly string? _app;
    private readonly string? _environment;
    private readonly string? _instance;

    public AppMeta(Type mainAssemblyType, string? app = null, string? environment = null, string? instance = null)
    {
        _mainAssemblyType = mainAssemblyType;
        _app = app;
        _environment = environment;
        _instance = instance;
    }

    public string Version() => _mainAssemblyType.Assembly
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
        .InformationalVersion ?? Constants.NoValue;

    public string App
    {
        get
        {
            if (_app == null)
                throw new ArgumentNullException(nameof(App));
            return _app;
        }
    }

    public string? Environment => _environment;

    public string? Instance => _instance;
}
