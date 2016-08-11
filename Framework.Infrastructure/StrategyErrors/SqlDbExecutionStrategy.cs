using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Infrastructure.StrategyErrors
{
    /// <summary>
    /// Class that processes strategy for sql exceptions.
    /// </summary>
    public class SqlDbExecutionStrategy : DbExecutionStrategy, ITransientErrorDetectionStrategy
    {
        /// <summary>
        /// Public constructor.
        /// </summary>
        public SqlDbExecutionStrategy() { }
        /// <summary>
        /// Constructor with max delay and timespan.
        /// </summary>
        /// <param name="maxRetryCount">Max retry count.</param>
        /// <param name="maxDelay">Max delay between retries.</param>
        public SqlDbExecutionStrategy(int maxRetryCount, TimeSpan maxDelay)
            : base(maxRetryCount, maxDelay)
        {
        }
        /// <summary>
        /// Indicates if is a SqlException.
        /// </summary>
        /// <param name="ex">An exception.</param>
        /// <returns>True if SqlException, false otherwise.</returns>
        public bool IsTransient(Exception ex)
        {
            if (ex != null)
            {
                SqlException exception = ex as SqlException;
                if (exception != null)
                {
                    foreach (SqlError error in exception.Errors)
                    {
                        switch (error.Number)
                        {
                            case 0x2745:
                            case 0x2746:
                            case 0x274c:
                            case 0xe9:
                            case 20:
                            case 0x40:
                            case 0x2ab0:
                            case 0x2ab1:
                            case 0x9ccf:
                            case 0x9d05:
                            case 0x9e5c:
                            case 0x9ea5:
                                return true;

                            case 0x9e35:
                                {
                                    ThrottlingCondition condition = ThrottlingCondition.FromError(error);
                                    exception.Data[condition.ThrottlingMode.GetType().Name] = condition.ThrottlingMode.ToString();
                                    exception.Data[condition.GetType().Name] = condition;
                                    return true;
                                }
                        }
                    }
                }
                else
                {
                    if (ex is TimeoutException)
                    {
                        return true;
                    }
                    EntityException exception2 = ex as EntityException;
                    if (exception2 != null)
                    {
                        return this.IsTransient(exception2.InnerException);
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Indicates if exception must to be processed.
        /// </summary>
        /// <param name="exception">Exception to be processed.</param>
        /// <returns>Indicates if must to retry.</returns>
        protected override bool ShouldRetryOn(Exception exception)
        {
            return IsTransient(exception);
        }
    }
}
