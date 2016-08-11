using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Utility;
using RaraAvis.nCubed.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Logging
{
    /// <summary>
    /// Framework logging.
    /// </summary>
    /// <typeparam name="T">EventSource type.</typeparam>
    public sealed class FrameworkLoggingFactory<T> where T : FrameworkEventSource, new()
    {
        /// <summary>
        /// Listener for events.
        /// </summary>
        private static readonly ObservableEventListener listener = new ObservableEventListener();
        /// <summary>
        /// Instance for Lazy object.
        /// </summary>
        private static readonly Lazy<T> instance = new Lazy<T>(() =>
            {
                return new T();
            });
        /// <summary>
        /// 
        /// </summary>
        private FrameworkLoggingFactory() { }
        /// <summary>
        /// Object instance.
        /// </summary>
        public static T Instance
        {
            get
            {
#if DEBUG
                
                //EventSourceAnalyzer.InspectAll(instance.Value);
#endif
                return instance.Value;
            }
        }
        /// <summary>
        /// Enable events for this event source.
        /// </summary>
        public static void EnableEvents()
        {
            listener.EnableEvents(Instance, System.Diagnostics.Tracing.EventLevel.LogAlways, Keywords.All);
        }
        /// <summary>
        /// Disable events for this event source.
        /// </summary>
        public static void DisableEvens()
        {
            listener.DisableEvents(Instance);
        }
    }
}
