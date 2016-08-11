using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Messaging.StrategyErrors
{
    /// <summary>
    /// Class for transient error strategies.
    /// </summary>
    public class MessagingErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        /// <summary>
        /// Indicates if exception can to reply action.
        /// </summary>
        /// <param name="ex">Exception to match.</param>
        /// <returns>True if exception matches, false otherwise.</returns>
        public bool IsTransient(Exception ex)
        {
            return ex is Exception;
        }
    }
}
