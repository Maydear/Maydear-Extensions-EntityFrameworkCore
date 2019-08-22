using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maydear.Extensions.EntityFrameworkCore.Internal
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    internal static class LoggingExtension
    {
        private static readonly Action<ILogger, string, Exception> traceEntityLogging =
            LoggerMessage.Define<string>(
                                          eventId: 1,
                                          logLevel: LogLevel.Trace,
                                          formatString: "Failed to validate the token {Token}."
            );

        public static void LogTraceEntity(this ILogger logger, string functionName, object entity)
        {
            traceEntityLogging(logger, $"{functionName}:{entity.ToString()}", null);
        }
    }
}
