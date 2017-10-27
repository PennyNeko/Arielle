using System;
using Discord;
using Microsoft.Extensions.Logging;

namespace Arielle
{
    public interface ILogger
    {
        void Log(LogLevel logLevel, int v, LogMessage message, Exception exception, Func<object, object, string> p);
    }
}