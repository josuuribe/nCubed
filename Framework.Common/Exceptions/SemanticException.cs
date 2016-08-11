using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Exceptions
{
    /// <summary>
    /// Core exception that raises framework when error found.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly"), Serializable]
    public class SemanticException : Exception
    {
        /// <summary>
        /// Exception id that is being managed.
        /// </summary>
        public Guid HandlingInstanceId
        {
            get;
            private set;
        }
        /// <summary>
        /// Base constructor.
        /// </summary>
        public SemanticException()
            : base()
        {

        }
        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="message">This is ignored, message is "Semantic Exception".</param>
        public SemanticException(string message)
            : base("Semantic Exception")
        {
            this.HandlingInstanceId = Guid.Parse(message);
        }
        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="info">Additional info to add.</param>
        /// <param name="context">Context being processed.</param>
        protected SemanticException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="message">Message string, it is always equals to "Semantic Exception".</param>
        /// <param name="innerException">inner exception that produces this error.</param>
        public SemanticException(string message, Exception innerException)
            : base("Semantic Exception", innerException)
        {
            this.HandlingInstanceId = Guid.Parse(message);
        }
    }
}
