using RaraAvis.nCubed.Core.Configurations;
using RaraAvis.nCubed.Core.Configurations.Common.Sections;
using RaraAvis.nCubed.Core.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Exceptions
{
    /// <summary>
    /// A <see cref="T:System.Diagnostics.Tracing.EventSource"/> class to store core exception messages.
    /// </summary>
    [EventSource(Name = "Optimissa-N3-Core-Error")]
    public class CoreErrorEventSource : FrameworkEventSource
    {
        /// <summary>
        /// Public constructor for store core related errors.
        /// </summary>
        public CoreErrorEventSource()
        {
            try
            {
                N3Section section = ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section;
                CoreErrorEventSource.SetCurrentThreadActivityId(section.System.ActivityId);
            }
            catch(System.NullReferenceException nre)
            {
                throw new ArgumentNullException(Resources.ExceptionSystemConfigurationMissing, nre);
            }
            
        }
        /// <summary>
        /// Logs infrastructure core errors related.
        /// </summary>
        /// <param name="relatedActivityId">Related ActivityId.</param>
        /// <param name="formattedException">Exception formatted, a string that describes the exception.</param>
        [Event(1101, Opcode = EventOpcode.Send, Task = Tasks.Infrastructure, Level = EventLevel.Error, Message = "{0}")]
        public void LogInfrastructureException(Guid relatedActivityId, string formattedException)
        {
            WriteEventWithRelatedActivityId(1101, relatedActivityId, formattedException);
        }
        /// <summary>
        /// Logs core errors related.
        /// </summary>
        /// <param name="relatedActivityId">Related ActivityId.</param>
        /// <param name="formattedException">Exception formatted, a string that describes the exception.</param>
        [Event(1102, Opcode = EventOpcode.Send, Task = Tasks.Core, Level = EventLevel.Error, Message = "{0}")]
        public void LogCoreException(Guid relatedActivityId, string formattedException)
        {
            WriteEventWithRelatedActivityId(1102, relatedActivityId, formattedException);
        }
    }
}
