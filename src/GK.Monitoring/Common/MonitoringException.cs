using System;

namespace GK.Monitoring.Common;

public class MonitoringException : Exception
{
    public MonitoringException(string message) : base(message)
    {
    }
}
