using RaraAvis.nCubed.Core.Configurations;
using RaraAvis.nCubed.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Infrastructure.Logging
{
    /// <summary>
    /// A <see cref="T:System.Diagnostics.Tracing.EventSource"/> class to store data access logging messages.
    /// </summary>
    [EventSource(Name = "RaraAvis-N3-Database-Logging")]
    public class DatabaseLoggingEventSource : FrameworkEventSource
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public DatabaseLoggingEventSource()
        {
            N3Section section = ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section;
            DatabaseLoggingEventSource.SetCurrentThreadActivityId(section.System.DatabaseConfiguration.ActivityId);
        }
        /// <summary>
        /// Traces an external API.
        /// </summary>
        /// <param name="componentName">Component traced.</param>
        /// <param name="method">Method traced.</param>
        /// <param name="timespan">Log timespan.</param>
        /// <param name="command">Command to log.</param>
        [NonEvent]
        public void TraceApi(string componentName, string method, TimeSpan timespan, DbCommand command)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<SqlTrace>");

            sb.Append("<ComponentName>");
            sb.Append(componentName);
            sb.Append("</ComponentName>");

            sb.Append("<Method>");
            sb.Append(method);
            sb.Append("</Method>");

            sb.Append("<TimeSpan>");
            sb.Append(timespan);
            sb.Append("</TimeSpan>");

            sb.Append("<Command>");
            sb.Append("<CommandText>");
            sb.Append(command.CommandText);
            sb.Append("</CommandText>");
            sb.Append("<Timeout>");
            sb.Append(command.CommandTimeout);
            sb.Append("</Timeout>");
            sb.Append("<Type>");
            sb.Append(command.CommandType);
            sb.Append("</Type>");

            sb.Append("<Connection>");
            sb.Append("<ConnectionString>");
            sb.Append(command.Connection.ConnectionString);
            sb.Append("</ConnectionString>");

            sb.Append("<ConnectionTimeout>");
            sb.Append(command.Connection.ConnectionTimeout);
            sb.Append("</ConnectionTimeout>");

            sb.Append("<Database>");
            sb.Append(command.Connection.Database);
            sb.Append("</Database>");

            sb.Append("<DataSource>");
            sb.Append(command.Connection.DataSource);
            sb.Append("</DataSource>");
            sb.Append("</Connection>");

            sb.Append("<Parameters>");
            foreach (DbParameter parameter in command.Parameters)
            {
                sb.Append("<Parameter>");

                sb.Append("<Type>");
                sb.Append(parameter.DbType);
                sb.Append("</Type>");

                sb.Append("<Direction>");
                sb.Append(parameter.Direction);
                sb.Append("</Direction>");

                sb.Append("<Name>");
                sb.Append(parameter.ParameterName);
                sb.Append("</Name>");

                sb.Append("<Precision>");
                sb.Append(parameter.Precision);
                sb.Append("</Precision>");

                sb.Append("<Scale>");
                sb.Append(parameter.Scale);
                sb.Append("</Scale>");

                sb.Append("<Size>");
                sb.Append(parameter.Size);
                sb.Append("</Size>");

                sb.Append("<SourceColumn>");
                sb.Append(parameter.SourceColumn);
                sb.Append("</SourceColumn>");

                sb.Append("<SourceVersion>");
                sb.Append(parameter.SourceVersion);
                sb.Append("</SourceVersion>");

                sb.Append("<Value>");
                sb.Append(parameter.Value);
                sb.Append("</Value>");

                sb.Append("</Parameter>");
            }
            sb.Append("</Parameters>");
            sb.Append("</Command>");
            sb.Append("</SqlTrace>");
            TraceApi(sb.ToString());
        }
        [Event(103, Level = EventLevel.Informational, Message = "{0}")]
        internal void TraceApi(string message)
        {
            WriteEvent(103, message);
        }
    }
}
