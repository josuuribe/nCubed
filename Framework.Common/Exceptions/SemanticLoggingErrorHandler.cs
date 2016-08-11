using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using RaraAvis.nCubed.Core.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Exceptions
{
    /// <summary>
    /// Class that handles semantic errors.
    /// </summary>
    public class SemanticLoggingErrorHandler : IExceptionHandler
    {
        /// <summary>
        /// Action to be processed.
        /// </summary>
        private Action<Guid, string> writeToLog;
        /// <summary>
        /// Formatter for this errors.
        /// </summary>
        private Func<TextWriter, Exception, Guid, ExceptionFormatter> formatterCreator;
        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="writeToLog">Action that writes log.</param>
        public SemanticLoggingErrorHandler(Action<Guid, string> writeToLog)
            : this(writeToLog, null)
        { }
        /// <summary>
        /// Base constructor with formatter.
        /// </summary>
        /// <param name="writeToLog">Action that writes log.</param>
        /// <param name="formatterType">Formatter type to be used.</param>
        public SemanticLoggingErrorHandler(Action<Guid, string> writeToLog, Type formatterType)
        {
            Guard.ArgumentNotNull(writeToLog, "writeToLog");

            if (formatterType != null)
            {
                if (!typeof(ExceptionFormatter).IsAssignableFrom(formatterType))
                {
                    throw new ArgumentOutOfRangeException("formatterType", "formatterType must be of type ExceptionFormatter");
                }

                this.formatterCreator = GetFormatterCreator(formatterType);
            }

            this.writeToLog = writeToLog;
        }
        /// <summary>
        /// Writes exception to log.
        /// </summary>
        /// <param name="exception">Exception to be logged.</param>
        /// <param name="handlingInstanceId">Id to be processed.</param>
        /// <returns>Exception to be processed.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "It is used by EntLib Infrastructure")]
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            if (exception.GetType() == typeof(SemanticException))
            {
                writeToLog((exception as SemanticException).HandlingInstanceId,
                    CreateMessage(exception, handlingInstanceId));
            }
            else
            {
                writeToLog(handlingInstanceId,
                    CreateMessage(exception, handlingInstanceId));
            }
            return exception;
        }
        /// <summary>
        /// Creates a exception message.
        /// </summary>
        /// <param name="exception">Exception to be logged.</param>
        /// <param name="handlingInstanceID">Exception Id.</param>
        /// <returns>A string to be logged</returns>
        private string CreateMessage(Exception exception, Guid handlingInstanceID)
        {
            if (formatterCreator != null)
            {
                using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
                {
                    ExceptionFormatter formatter = formatterCreator(writer, exception, handlingInstanceID);

                    if (formatter != null)
                    {
                        formatter.Format();
                    }

                    return writer.ToString();
                }
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Gets formatter creator.
        /// </summary>
        /// <param name="formatterType">Formatter type to be used.</param>
        /// <returns>Function that processes exceptions.</returns>
        private Func<TextWriter, Exception, Guid, ExceptionFormatter> GetFormatterCreator(Type formatterType)
        {
            ConstructorInfo ctor = formatterType.GetConstructor(new Type[] { 
                typeof(TextWriter), typeof(Exception), typeof(Guid) });

            if (ctor == null)
            {
                throw new ExceptionHandlingException(
                    string.Format(System.Globalization.CultureInfo.InvariantCulture,Resources.ExceptionFormatterError, formatterType.AssemblyQualifiedName));
            }

            ParameterExpression[] argsExp = new ParameterExpression[]
            { 
                Expression.Parameter(typeof(TextWriter)),
                Expression.Parameter(typeof(Exception)),
                Expression.Parameter(typeof(Guid))
            };

            NewExpression newExp = Expression.New(ctor, argsExp);

            var lambda = Expression.Lambda<Func<TextWriter, Exception, Guid, ExceptionFormatter>>(newExp, argsExp);
            formatterCreator = lambda.Compile();

            return formatterCreator;
        }
    }
}
