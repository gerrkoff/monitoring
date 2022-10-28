using System;

namespace GerrKoff.Monitoring.Common;

public class MonitoringException : Exception
{
    public MonitoringException(string message) : base(message)
    {
    }
}
