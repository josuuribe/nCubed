using RaraAvis.nCubed.Core.Configurations;
using RaraAvis.nCubed.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Logging
{
    /// <summary>
    /// A <see cref="T:System.Diagnostics.Tracing.EventSource"/> class to store core logging messages.
    /// </summary>
    [EventSource(Name = "RaraAvis-N3-Core-Logging")]
    public class FrameworkLoggingEventSource : FrameworkEventSource
    {
        /// <summary>
        /// Public constructor for store core related events.
        /// </summary>
        public FrameworkLoggingEventSource()
        {
            N3Section section = ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section;
            FrameworkLoggingEventSource.SetCurrentThreadActivityId(section.System.LogId);
        }
        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        [Event(1201, Opcode = EventOpcode.Send, Task = Tasks.Logging, Level = EventLevel.Verbose, Message = "{0}")]
        internal void LogMessage(string message)
        {
            WriteEvent(1201, message);
        }
        /// <summary>
        /// Logs a message correlation Id given.
        /// </summary>
        /// <param name="correlated">Correlation id associated.</param>
        /// <param name="message">Message to log.</param>
        [Event(1202, Opcode = EventOpcode.Send, Task = Tasks.Logging, Level = EventLevel.Verbose, Message = "{0}")]
        internal void LogCorrelatedMessage(Guid correlated, string message)
        {
            WriteEventWithRelatedActivityId(1202, correlated, message);
        }
    }
}
