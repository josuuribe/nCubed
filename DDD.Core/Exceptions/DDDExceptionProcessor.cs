using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using RaraAvis.nCubed.Core.Configurations;
using RaraAvis.nCubed.Core.Exceptions;
using RaraAvis.nCubed.Core.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Core.Exceptions
{
    /// <summary>
    /// Class that processes exceptions.
    /// </summary>
    public static class DDDExceptionProcessor
    {
        private enum POLICIES { INFRASTRUCTURE_POLICY, DOMAIN_POLICY, APPLICATION_POLICY, WEBSERVER_POLICY }
        private static readonly DDDErrorEventSource Log = FrameworkLoggingFactory<DDDErrorEventSource>.Instance;
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
        /// Tries to manage a exception from domain.
        /// </summary>
        /// <param name="exceptionToHandle">Exception tries to manage.</param>
        /// <param name="exceptionToThrow">Returns exception handled if possible.</param>
        /// <returns>True if exception is handled, false otherwise.</returns>
        public static bool HandleExceptionDomain(Exception exceptionToHandle, out Exception exceptionToThrow)
        {
            return ExceptionManager.HandleException(exceptionToHandle, POLICIES.DOMAIN_POLICY.ToString(), out exceptionToThrow);
        }
        /// <summary>
        /// Tries to manage a exception from application service.
        /// </summary>
        /// <param name="exceptionToHandle">Exception to be manage.</param>
        /// <param name="exceptionToThrow">Exception that will be returned politic based.</param>
        /// <returns>True if manage, false otherwise.</returns>
        public static bool HandleExceptionApplication(Exception exceptionToHandle, out Exception exceptionToThrow)
        {
            return ExceptionManager.HandleException(exceptionToHandle, POLICIES.APPLICATION_POLICY.ToString(), out exceptionToThrow);
        }
        /// <summary>
        /// Tries to manage a exception from repositories.
        /// </summary>
        /// <param name="exceptionToHandle">Exception to be manage.</param>
        /// <param name="exceptionToThrow">Exception that will be returned politic based.</param>
        /// <returns>True if manage, false otherwise.</returns>
        public static bool HandleExceptionDataAccess(Exception exceptionToHandle, out Exception exceptionToThrow)
        {
            return ExceptionManager.HandleException(exceptionToHandle, POLICIES.INFRASTRUCTURE_POLICY.ToString(), out exceptionToThrow);
        }
        /// <summary>
        /// Tries to manage a exception from domain.
        /// </summary>
        /// <param name="exceptionToHandle">Exception to be manage.</param>
        /// <returns>True if manage, false otherwise.</returns>
        public static bool HandleExceptionDomain(Exception exceptionToHandle)
        {
            return ExceptionManager.HandleException(exceptionToHandle, POLICIES.DOMAIN_POLICY.ToString());
        }
        /// <summary>
        /// Tries to manage a exception before show user.
        /// </summary>
        /// <param name="exceptionToHandle">Exception to be manage.</param>
        /// <returns>True if manage, false otherwise.</returns>
        public static bool HandleExceptionWebserver(Exception exceptionToHandle)
        {
            return ExceptionManager.HandleException(exceptionToHandle, POLICIES.WEBSERVER_POLICY.ToString());
        }
        /// <summary>
        /// Tries to manage a exception from domain.
        /// </summary>
        /// <param name="action">Method to be processed.</param>
        public static void ProcessDomain(Action action)
        {
            ExceptionManager.Process(action, POLICIES.DOMAIN_POLICY.ToString());
        }
        /// <summary>
        /// Tries to manage a exception from application service.
        /// </summary>
        /// <typeparam name="TResult">Type to be returned.</typeparam>
        /// <param name="action">Method to be processed.</param>
        /// <returns>Value to be returned by function execution.</returns>
        public static TResult ProcessApplication<TResult>(Func<TResult> action)
        {
            return ExceptionManager.Process(action, POLICIES.APPLICATION_POLICY.ToString());
        }
        /// <summary>
        /// Tries to manage a exception from application service.
        /// </summary>
        /// <param name="action">Method to be processed.</param>
        public static void ProcessApplication(Action action)
        {
            ExceptionManager.Process(action, POLICIES.APPLICATION_POLICY.ToString());
        }
        /// <summary>
        /// Tries to manage a exception from repositories.
        /// </summary>
        /// <typeparam name="TResult">Type to be returned.</typeparam>
        /// <param name="action">Method to be processed.</param>
        /// <returns>Value to be returned by function execution.</returns>
        public static TResult ProcessDataAccess<TResult>(Func<TResult> action)
        {
            return ExceptionManager.Process<TResult>(action, POLICIES.INFRASTRUCTURE_POLICY.ToString());
        }
        /// <summary>
        /// Tries to manage a exception from repositories.
        /// </summary>
        /// <param name="action">Method to be processed.</param>
        public static void ProcessDataAccess(Action action)
        {
            ExceptionManager.Process(action, POLICIES.INFRASTRUCTURE_POLICY.ToString());
        }
        /// <summary>
        /// Tries to manage a exception before show user.
        /// </summary>
        /// <param name="action">Method to be processed.</param>
        public static void ProcessWebserver(Action action)
        {
            ExceptionManager.Process(action, POLICIES.WEBSERVER_POLICY.ToString());
        }
        /// <summary>
        /// Tries to manage a exception from domain.
        /// </summary>
        /// <typeparam name="TResult">Type to be returned.</typeparam>
        /// <param name="action">Method to be processed.</param>
        /// <returns>Object defined.</returns>
        public static TResult ProcessDomain<TResult>(Func<TResult> action)
        {
            return ExceptionManager.Process<TResult>(action, POLICIES.DOMAIN_POLICY.ToString());
        }
        /// <summary>
        /// Tries to manage a exception before show user.
        /// </summary>
        /// <typeparam name="TResult">Type to be returned.</typeparam>
        /// <param name="action">Method to be processed.</param>
        /// <returns>Object defined.</returns>
        public static TResult ProcessWebserver<TResult>(Func<TResult> action)
        {
            return ExceptionManager.Process<TResult>(action, POLICIES.WEBSERVER_POLICY.ToString());
        }
        /// <summary>
        /// Tries to manage a exception from domain.
        /// </summary>
        /// <typeparam name="TResult">Type to be returned.</typeparam>
        /// <param name="action">Method to be processed.</param>
        /// <param name="defaultResult">Default value to supply.</param>
        /// <returns>Object defined.</returns>
        public static TResult ProcessDomain<TResult>(Func<TResult> action, TResult defaultResult)
        {
            return ExceptionManager.Process<TResult>(action, defaultResult, POLICIES.DOMAIN_POLICY.ToString());
        }
        /// <summary>
        /// Tries to manage a exception before show user.
        /// </summary>
        /// <typeparam name="TResult">Type to be returned.</typeparam>
        /// <param name="action">Method to be processed.</param>
        /// <param name="defaultResult">Default value to supply.</param>
        /// <returns>Object defined.</returns>
        public static TResult ProcessWebserver<TResult>(Func<TResult> action, TResult defaultResult)
        {
            return ExceptionManager.Process<TResult>(action, defaultResult, POLICIES.WEBSERVER_POLICY.ToString());
        }
        /// <summary>
        /// Creates exception configuration section programatically.
        /// </summary>
        /// <returns>A exception manager object.</returns>
        public static ExceptionManager BuildExceptionManagerConfig()
        {
            var policies = new List<ExceptionPolicyDefinition>();

            var expressivePolicy = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(typeof (Exception),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                        new SemanticLoggingErrorHandler(Log.LogDataAccessException, typeof(XmlExceptionFormatter)),
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
                        new SemanticLoggingErrorHandler(Log.LogDomainException, typeof(XmlExceptionFormatter)),
                     }),
            };

            var securePolicy = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(typeof (Exception),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                        new SemanticLoggingErrorHandler(Log.LogApplicationException, typeof(XmlExceptionFormatter)),
                        new ReplaceHandler("{handlingInstanceID}",
                         typeof(SemanticException))
                     }),
            };

            var mappings = new NameValueCollection();
            mappings.Add("FaultID", "{Guid}");
            mappings.Add("FaultMessage", "{Message}");

            var webServicePolicy = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(typeof (SemanticException),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                        new FaultContractExceptionHandler(typeof(WebServiceFault), "Webservice Error. Please contact your administrator", mappings)
                     })
            };

            policies.Add(new ExceptionPolicyDefinition(POLICIES.INFRASTRUCTURE_POLICY.ToString(), expressivePolicy));
            policies.Add(new ExceptionPolicyDefinition(POLICIES.DOMAIN_POLICY.ToString(), basePolicy));
            policies.Add(new ExceptionPolicyDefinition(POLICIES.APPLICATION_POLICY.ToString(), securePolicy));
            policies.Add(new ExceptionPolicyDefinition(POLICIES.WEBSERVER_POLICY.ToString(), webServicePolicy));

            return new ExceptionManager(policies);
        }
    }
}
