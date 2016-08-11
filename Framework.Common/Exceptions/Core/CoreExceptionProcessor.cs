using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using RaraAvis.nCubed.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Exceptions.Core
{
    /// <summary>
    /// Class that processes exceptions.
    /// </summary>
    public static class CoreExceptionProcessor
    {
        private enum POLICIES { INFRASTRUCTURE_POLICY, CORE_POLICY }
        private static readonly CoreErrorEventSource Log = FrameworkLoggingFactory<CoreErrorEventSource>.Instance;
        private static ExceptionManager exceptionManager = BuildExceptionManagerConfig();
        /// <summary>
        /// EntLib ExceptionManager.
        /// </summary>
        public static ExceptionManager ExceptionManager
        {
            get
            {
                return exceptionManager;
            }
        }
        /// <summary>
        /// Handles a core exception given, and returns a processed exception, generally a <see cref="T:RaraAvis.nCubed.Core.Exceptions.SemanticException"/>
        /// </summary>
        /// <param name="exceptionToHandle">Exception that is going to be processed.</param>
        /// <param name="exceptionToThrow">Returns a <see cref="T:System.Exception"/>, it depends on configuration.</param>
        /// <returns>True if exception is handled, False otherwise.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "It is used by EntLib Infrastructure")]
        public static bool HandleExceptionCore(Exception exceptionToHandle, out Exception exceptionToThrow)
        {
            return ExceptionManager.HandleException(exceptionToHandle, POLICIES.CORE_POLICY.ToString(), out exceptionToThrow);
        }
        /// <summary>
        /// Handles an infrastructure exception given, and returns a processed exception, generally a <see cref="T:RaraAvis.nCubed.Core.Exceptions.SemanticException"/>
        /// </summary>
        /// <param name="exceptionToHandle">Exception that is going to be processed.</param>
        /// <param name="exceptionToThrow">Returns a <see cref="T:System.Exception"/>, it depends on configuration.</param>
        /// <returns>True if exception is handled, False otherwise.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "It is used by EntLib Infrastructure")]
        public static bool HandleExceptionDataSource(Exception exceptionToHandle, out Exception exceptionToThrow)
        {
            return ExceptionManager.HandleException(exceptionToHandle, POLICIES.INFRASTRUCTURE_POLICY.ToString(), out exceptionToThrow);
        }
        /// <summary>
        /// Handles a core exception given.
        /// </summary>
        /// <param name="exceptionToHandle">Exception that is going to be processed.</param>
        /// <returns>True if exception is handled, False otherwise.</returns>
        public static bool HandleExceptionCore(Exception exceptionToHandle)
        {
            return ExceptionManager.HandleException(exceptionToHandle, POLICIES.CORE_POLICY.ToString());
        }
        /// <summary>
        /// Handles an infrastructure exception given, and returns a processed exception, generally a <see cref="T:RaraAvis.nCubed.Core.Exceptions.SemanticException"/>
        /// </summary>
        /// <param name="exceptionToHandle">Exception that is going to be processed.</param>
        /// <returns>True if exception is handled, False otherwise.</returns>
        public static bool HandleExceptionDataSource(Exception exceptionToHandle)
        {
            return ExceptionManager.HandleException(exceptionToHandle, POLICIES.INFRASTRUCTURE_POLICY.ToString());
        }
        /// <summary>
        /// Executes a core <see cref="T:System.Action"/> and processes any exception raised.
        /// </summary>
        /// <param name="action">A <see cref="T:System.Action"/> to be processed.</param>
        public static void ProcessCore(Action action)
        {
            ExceptionManager.Process(action, POLICIES.CORE_POLICY.ToString());
        }
        /// <summary>
        /// Executes an infrastructure <see cref="T:System.Action"/> and processes any exception raised.
        /// </summary>
        /// <param name="action">A <see cref="T:System.Action"/> to be processed.</param>
        public static void ProcessInfrastructure(Action action)
        {
            ExceptionManager.Process(action, POLICIES.INFRASTRUCTURE_POLICY.ToString());
        }
        /// <summary>
        /// Executes a core <see cref="T:System.Func`1"/> and processes any exception raised.
        /// </summary>
        /// <typeparam name="TResult">Result type to be returned.</typeparam>
        /// <param name="action">A <see cref="T:System.Func`1"/> to be processed.</param>
        /// <returns>The value returned by method.</returns>
        public static TResult ProcessCore<TResult>(Func<TResult> action)
        {
            return ExceptionManager.Process<TResult>(action, POLICIES.CORE_POLICY.ToString());
        }
        /// <summary>
        /// Executes an infrastructure <see cref="T:System.Func`1"/> and processes any exception raised.
        /// </summary>
        /// <typeparam name="TResult">Result type to be returned.</typeparam>
        /// <param name="action">A <see cref="T:System.Func`1"/> to be processed.</param>
        /// <returns>The value returned by method.</returns>
        public static TResult ProcessInfrastructure<TResult>(Func<TResult> action)
        {
            return ExceptionManager.Process<TResult>(action, POLICIES.INFRASTRUCTURE_POLICY.ToString());
        }
        private static ExceptionManager BuildExceptionManagerConfig()
        {
            var policies = new List<ExceptionPolicyDefinition>();

            var expressivePolicy = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(typeof (Exception),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                        new SemanticLoggingErrorHandler(Log.LogInfrastructureException, typeof(XmlExceptionFormatter)),
                        new WrapHandler("{handlingInstanceID}",
                         typeof(SemanticException))
                     })
            };

            var basePolicy = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(typeof (Exception),
                    PostHandlingAction.NotifyRethrow,
                    new IExceptionHandler[]
                     {
                        new SemanticLoggingErrorHandler(Log.LogCoreException, typeof(XmlExceptionFormatter)),
                     }),
            };

            policies.Add(new ExceptionPolicyDefinition(POLICIES.INFRASTRUCTURE_POLICY.ToString(), expressivePolicy));
            policies.Add(new ExceptionPolicyDefinition(POLICIES.CORE_POLICY.ToString(), basePolicy));

            return new ExceptionManager(policies);
        }
    }
}
