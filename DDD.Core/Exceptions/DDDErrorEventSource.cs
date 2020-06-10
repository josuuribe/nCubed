using RaraAvis.nCubed.Core.Configurations;
using RaraAvis.nCubed.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Core.Exceptions
{
    /// <summary>
    /// A <see cref="T:System.Diagnostics.Tracing.EventSource"/> class to store DDD messages.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DDD")]
    [EventSource(Name = "RaraAvis-N3-DDD-Error")]
    public class DDDErrorEventSource : FrameworkEventSource
    {
        /// <summary>
        /// Public constructor for store DDD related events.
        /// </summary>
        public DDDErrorEventSource()
        {
            N3Section section = ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section;
            DDDErrorEventSource.SetCurrentThreadActivityId(section.DDD.ActivityId);
        }
        /// <summary>
        /// Logs a data access exception.         
        /// </summary>
        /// <param name="relatedActivityId">Handling Entlib exception Id.</param>
        /// <param name="formattedException">Formatted exception.</param>
        [Event(2101, Opcode = EventOpcode.Send, Task = Tasks.Infrastructure, Level = EventLevel.Error, Message = "{0}")]
        internal void LogDataAccessException(Guid relatedActivityId, string formattedException)
        {
            WriteEventWithRelatedActivityId(2101, relatedActivityId, formattedException);
        }
        /// <summary>
        /// Logs a domain exception.
        /// </summary>
        /// <param name="relatedActivityId">Handling Entlib exception Id.</param>
        /// <param name="formattedException">Formatted exception.</param>

        [Event(2102, Opcode = EventOpcode.Send, Task = Tasks.Core, Level = EventLevel.Error, Message = "{0}")]
        internal void LogDomainException(Guid relatedActivityId, string formattedException)
        {
            WriteEventWithRelatedActivityId(2102, relatedActivityId, formattedException);
        }
        /// <summary>
        /// Logs an application service exception.
        /// </summary>
        /// <param name="relatedActivityId">Handling Entlib exception Id.</param>
        /// <param name="formattedException">Formatted exception.</param>

        [Event(2103, Opcode = EventOpcode.Send, Task = Tasks.Core, Level = EventLevel.Error, Message = "{0}")]
        internal void LogApplicationException(Guid relatedActivityId, string formattedException)
        {
            WriteEventWithRelatedActivityId(2103, relatedActivityId, formattedException);
        }
    }
}
