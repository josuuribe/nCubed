using RaraAvis.nCubed.Core.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Core.Services
{
    /// <summary>
    /// Base class for DTO entities.
    /// </summary>
    [DataContract]
    public abstract class EntityDto
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        protected EntityDto() { }

    }
}
