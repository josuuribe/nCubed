using RaraAvis.nCubed.Core.Configurations;
using RaraAvis.nCubed.Core.Configurations.Common.Sections;
using RaraAvis.nCubed.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.CQRS.Core.Exceptions
{
    /// <summary>
    /// A <see cref="T:System.Diagnostics.Tracing.EventSource"/> class to store CQRS messages.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CQRS")]
    [EventSource(Name = "Optimissa-N3-CQRS-Error")]
    public class CQRSErrorEventSource : FrameworkEventSource
    {
        /// <summary>
        /// Public constructor for store CQRS related events.
        /// </summary>
        public CQRSErrorEventSource()
        {
            N3Section section = ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section;
            CQRSErrorEventSource.SetCurrentThreadActivityId(section.CQRS.ActivityId);
        }
        /// <summary>
        /// Logs exceptions CQRS related.
        /// </summary>
        /// <param name="relatedActivityId">Related ActivityId.</param>
        /// <param name="formattedException">Exception formatted, a string that describes the exception.</param>
        [Event(3101, Opcode = EventOpcode.Send, Task = Tasks.Core, Level = EventLevel.Error, Message = "{0}")]
        public void LogException(Guid relatedActivityId, string formattedException)
        {
            WriteEventWithRelatedActivityId(3101, relatedActivityId, formattedException);
        }
    }
}
