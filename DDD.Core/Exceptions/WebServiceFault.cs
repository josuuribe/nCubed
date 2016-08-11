using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Core.Exceptions
{
    /// <summary>
    /// Class for send exception information to clients.
    /// </summary>
    [DataContract]
    public class WebServiceFault
    {
        /// <summary>
        /// Fault Id.
        /// </summary>
        [DataMember]
        public Guid FaultId { get; set; }
        /// <summary>
        /// Fault message.
        /// </summary>
        [DataMember]
        public string FaultMessage { get; set; }
    }
}
