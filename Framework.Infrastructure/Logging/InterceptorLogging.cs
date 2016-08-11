using RaraAvis.nCubed.Core.Exceptions.Core;
using RaraAvis.nCubed.Core.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Infrastructure.Logging
{
    /// <summary>
    /// Class that intercepts database access log.
    /// </summary>
    internal class InterceptorLogging : DbCommandInterceptor
    {
        private readonly DatabaseLoggingEventSource logger = FrameworkLoggingFactory<DatabaseLoggingEventSource>.Instance;
        private readonly Stopwatch stopwatch = new Stopwatch();
        /// <summary>
        /// Scalar executing.
        /// </summary>
        /// <param name="command">Command to log.</param>
        /// <param name="interceptionContext">Interception context.</param>
        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            base.ScalarExecuting(command, interceptionContext);
            stopwatch.Restart();
        }
        /// <summary>
        /// Scalar executed.
        /// </summary>
        /// <param name="command">Command to log.</param>
        /// <param name="interceptionContext">Interception context.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "It is used by EF Infrastructure")]
        public override void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                CoreExceptionProcessor.HandleExceptionDataSource(interceptionContext.Exception);
            }
            else
            {
                logger.TraceApi("SQL Database", "Interceptor.ScalarExecuted", stopwatch.Elapsed, command);
            }
            base.ScalarExecuted(command, interceptionContext);
        }
        /// <summary>
        /// Executing non query.
        /// </summary>
        /// <param name="command">Command to log.</param>
        /// <param name="interceptionContext">Interception context.</param>
        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            base.NonQueryExecuting(command, interceptionContext);
            stopwatch.Restart();
        }
        /// <summary>
        /// Executing non query.
        /// </summary>
        /// <param name="command">Command to log.</param>
        /// <param name="interceptionContext">Interception context.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "It is used by EF Infrastructure")]
        public override void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                CoreExceptionProcessor.HandleExceptionDataSource(interceptionContext.Exception);
            }
            else
            {
                logger.TraceApi("SQL Database", "Interceptor.NonQueryExecuted", stopwatch.Elapsed, command);
            }
            base.NonQueryExecuted(command, interceptionContext);
        }
        /// <summary>
        /// Executing reader.
        /// </summary>
        /// <param name="command">Command to log.</param>
        /// <param name="interceptionContext">Interception context.</param>
        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            base.ReaderExecuting(command, interceptionContext);
            stopwatch.Restart();
        }
        /// <summary>
        /// Executed reader.
        /// </summary>
        /// <param name="command">Command to log.</param>
        /// <param name="interceptionContext">Interception context.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "It is used by EF Infrastructure")]
        public override void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                CoreExceptionProcessor.HandleExceptionDataSource(interceptionContext.Exception);
            }
            else
            {
                logger.TraceApi("SQL Database", "Interceptor.ReaderExecuted", stopwatch.Elapsed, command);
            }
            base.ReaderExecuted(command, interceptionContext);
        }
    }
}
