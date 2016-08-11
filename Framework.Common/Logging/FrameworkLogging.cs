using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Logging
{
    /// <summary>
    /// Base class for framework logging.
    /// </summary>
    public static class FrameworkLogging
    {
        private static readonly FrameworkLoggingEventSource logger = FrameworkLoggingFactory<FrameworkLoggingEventSource>.Instance;
        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        public static void LogMessage(string message)
        {
            logger.LogMessage(message);
        }
        /// <summary>
        /// Logs a message with a correlationId given.
        /// </summary>
        /// <param name="correlationId">Correlation Id to associate with.</param>
        /// <param name="message">Message to be logged.</param>
        public static void LogMessage(Guid correlationId, string message)
        {
            logger.LogCorrelatedMessage(correlationId, message);
        }
    }
}
