using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Messaging.Messages
{
    /// <summary>
    /// Interface for messages.
    /// </summary>
    [InheritedExport(typeof(IMessage))]
    public interface IMessage
    {
        
    }
}
