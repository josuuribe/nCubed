using RaraAvis.nCubed.Core.Configurations;
using RaraAvis.nCubed.Core.Configurations.Common.Sections;
using RaraAvis.nCubed.Core.Exceptions;
using RaraAvis.nCubed.Core.Exceptions.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.EventSourcing.Core.Exceptions
{
    /// <summary>
    /// /// A <see cref="T:System.Diagnostics.Tracing.EventSource"/> class to store EventSourcing messages.
    /// </summary>
    [EventSource(Name = "Optimissa-N3-EventSourcing-Error")]
    public class EventSourcingErrorEventSource : FrameworkEventSource
    {
        /// <summary>
        /// Public constructor for store EventSourcing related events.
        /// </summary>
        public EventSourcingErrorEventSource()
        {
            N3Section section = ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section;
            EventSourcingErrorEventSource.SetCurrentThreadActivityId(section.ES.ActivityId);
        }
        /// <summary>
        /// Logs exceptions EventSourcing related.
        /// </summary>
        /// <param name="relatedActivityId">Related ActivityId.</param>
        /// <param name="formattedException">Exception formatted, a string that describes the exception.</param>
        [Event(4101, Opcode = EventOpcode.Send, Task = Tasks.Core, Level = EventLevel.Error, Message = "{0}")]
        internal void LogException(Guid relatedActivityId, string formattedException)
        {
            WriteEventWithRelatedActivityId(4101, relatedActivityId, formattedException);
        }
    }
}
