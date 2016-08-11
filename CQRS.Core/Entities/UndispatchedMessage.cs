using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.CQRS.Core.Entities
{
    /// <summary>
    /// Class for messages that are lost during processing.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Undispatched")]
    public class UndispatchedMessage
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public UndispatchedMessage()
        {
            this.CreatedTime = DateTime.Now;
        }
        /// <summary>
        /// Constructor with identifier.
        /// </summary>
        /// <param name="id">Unique identifier for each command.</param>
        public UndispatchedMessage(Guid id)
            : this()
        {
            this.Id = id;
        }
        /// <summary>
        /// Command Id.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Command information.
        /// </summary>
        public string Commands { get; set; }
        /// <summary>
        /// Time wich this command was created.
        /// </summary>
        public DateTime CreatedTime { get; set; }
    }
}
