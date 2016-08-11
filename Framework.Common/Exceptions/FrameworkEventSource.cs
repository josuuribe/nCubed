using RaraAvis.nCubed.Core.Configurations;
using RaraAvis.nCubed.Core.Configurations.Common.Sections;
using RaraAvis.nCubed.Core.Exceptions.Core;
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
    /// Base class for framework special configuration.
    /// </summary>
    public abstract class FrameworkEventSource : EventSource
    {
        /// <summary>
        /// Special keywords.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "It is used by EventSource Infrastructure")]
        public sealed class Keywords
        {
            private Keywords() { }
            /// <summary>
            /// Error.
            /// </summary>
            public const EventKeywords Error = (EventKeywords)1;
            /// <summary>
            /// Diagnostic.
            /// </summary>
            public const EventKeywords Diagnostic = (EventKeywords)2;
            /// <summary>
            /// Performance.
            /// </summary>
            public const EventKeywords Performance = (EventKeywords)4;
        }
        /// <summary>
        /// Special tasks.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public sealed class Tasks
        {
            private Tasks() { }
            /// <summary>
            /// Infrastructure.
            /// </summary>
            public const EventTask Infrastructure = (EventTask)1;
            /// <summary>
            /// Core.
            /// </summary>
            public const EventTask Core = (EventTask)2;
            /// <summary>
            /// Messaging.
            /// </summary>
            public const EventTask Messaging = (EventTask)3;
            /// <summary>
            /// Logging.
            /// </summary>
            public const EventTask Logging = (EventTask)4;
        }
    }
}
