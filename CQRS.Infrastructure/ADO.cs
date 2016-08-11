using EL = Microsoft.Practices.EnterpriseLibrary.Data;
using EF = System.Data.Entity;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.Entity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using RaraAvis.nCubed.Core.Exceptions;
using RaraAvis.nCubed.Core.Exceptions.Core;
using RaraAvis.nCubed.Core.Configurations;
using RaraAvis.nCubed.Core.Infrastructure.Logging;
using System;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace RaraAvis.nCubed.CQRS.Infrastructure
{
    /// <summary>
    /// Class that manages quick querys using ADO or CodeFirts mapping directy.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ADO", Justification = "It is so.")]
    public static class ADO
    {
        private static readonly Lazy<Stopwatch> stopwatch = new Lazy<Stopwatch>();
        private static readonly Lazy<DatabaseLoggingEventSource> logger = new Lazy<DatabaseLoggingEventSource>(() => new DatabaseLoggingEventSource());
        private static int sqlCommandTimeOut = (ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).System.DatabaseConfiguration.SqlCommandTimeout;
        /// <summary>
        /// Looks for connection string object.
        /// </summary>
        static ADO()
        {
            if (Assembly.GetEntryAssembly() != null)
            {
                var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().ManifestModule.Name);
                var section = config.GetSection("connectionStrings") as ConnectionStringsSection;
                if (section.SectionInformation.IsProtected)
                {
                    section.SectionInformation.UnprotectSection();
                }
                else
                {
                    section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                    config.Save();
                }
            }
            //TODO: Hace falta si no ha sido creado por otra parte de la aplicación
            DatabaseFactory.ClearDatabaseProviderFactory();
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
        }

        /// <summary>
        /// Creates a Database object for EntLib use.
        /// </summary>
        /// <param name="name">A name for the connection string.</param>
        /// <returns>A SqlDatabase object.</returns>
        public static SqlDatabase CreateDatabase(string name)
        {
            return DatabaseFactory.CreateDatabase(name) as SqlDatabase;
        }

        /// <summary>
        /// Gets the default database object.
        /// </summary>
        public static SqlDatabase DefaultDatabase
        {
            get
            {
                DatabaseProviderFactory dpf = new DatabaseProviderFactory();
                return dpf.CreateDefault() as SqlDatabase;
            }
        }

        private static bool IsLogEnabled
        {
            get
            {
                N3Section nCubeConfiguration = (ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section);
                return nCubeConfiguration.System.DatabaseConfiguration.LogSql;
            }
        }

        #region ·   DataTable   ·
        /// <summary>
        /// Processes a SQL query.
        /// </summary>
        /// <param name="sql">A string storing the sql clause.</param>
        /// <returns>A Datatable with the information.</returns>
        private static DataTable ExecuteView(string sql)
        {
            return CoreExceptionProcessor.ProcessInfrastructure<DataTable>(() =>
                {
                    var command = DefaultDatabase.GetSqlStringCommand(sql);
                    command.CommandTimeout = sqlCommandTimeOut;
                    using (var ds = new DataSet())
                    {
                        ds.Locale = System.Globalization.CultureInfo.CurrentCulture;
                        if (IsLogEnabled)
                        {
                            stopwatch.Value.Restart();
                            DefaultDatabase.LoadDataSet(command, ds, "View");
                            stopwatch.Value.Stop();
                            logger.Value.TraceApi("SQL Database", "ADO.ExecuteView", stopwatch.Value.Elapsed, command);
                        }
                        else
                        {
                            DefaultDatabase.LoadDataSet(command, ds, "View");
                        }
                        return ds.Tables["View"];
                    }
                });
        }

        /// <summary>
        /// Processes a SQL query.
        /// </summary>
        /// <param name="tableName">The table of the sql query.</param>
        /// <param name="fieldsAs">A Dictionary with provides the field and the alias for the field.</param>
        /// <param name="where">The where clause.</param>
        /// <returns>A Datatable with the information.</returns>
        public static DataTable GetTable(string tableName, Dictionary<string, string> fieldsAs, string where)
        {
            var sql = "SELECT ";
            foreach (var kvp in fieldsAs)
            {
                sql += kvp.Key + " AS " + kvp.Value + ",";
            }

            sql = sql.Substring(0, sql.Length - 1);

            sql += " FROM " + tableName + " WHERE " + where;

            return ExecuteView(sql);
        }

        /// <summary>
        /// Processes a SQL SELECT query that returns entire table.
        /// </summary>
        /// <param name="tableName">The table of the sql query.</param>
        /// <returns>A Datatable with the information.</returns>
        public static DataTable GetTable(string tableName)
        {
            var sql = "SELECT * FROM " + tableName;
            return ExecuteView(sql);
        }

        /// <summary>
        /// Processes a SQL query.
        /// </summary>
        /// <param name="tableName">The table of the sql query.</param>
        /// <param name="where">The where clause.</param>
        /// <returns>A Datatable with the information.</returns>
        public static DataTable GetTable(string tableName, string where)
        {
            var sql = "SELECT * FROM " + tableName + " WHERE " + where;
            return ExecuteView(sql);
        }

        /// <summary>
        /// Processes a SQL query using a funtion.
        /// </summary>
        /// <param name="view">The function to be executed.</param>
        /// <param name="fieldsAs">A Dictionary with provides the field and the alias for the field.</param>
        /// <param name="parameters">Parameters that must be passed to a function.</param>
        /// <returns>A Datatable with the information.</returns>
        public static DataTable GetTable(string view, Dictionary<string, string> fieldsAs, params string[] parameters)
        {
            StringBuilder sb = new StringBuilder("SELECT ");
            foreach (var kvp in fieldsAs)
            {
                sb.Append(kvp.Key);
                sb.Append(" AS ");
                sb.Append(kvp.Value);
            }

            sb.Append(" FROM ");
            sb.Append(view);
            sb.Append("('");

            foreach (string param in parameters)
            {
                sb.Append("'");
                sb.Append(param);
                sb.Append("',");
            }

            sb.Length = sb.Length - 1;
            return ExecuteView(sb.ToString());
        }
        #endregion

        #region ·   IDataReader ·
        /// <summary>
        /// Processes a SQL query.
        /// </summary>
        /// <param name="connectionString">A connection string.</param>
        /// <param name="sql">A sql query.</param>
        /// <returns>A IDataReader object.</returns>
        private static IDataReader ExecuteFunction(string connectionString, string sql)
        {
            return CoreExceptionProcessor.ProcessInfrastructure<IDataReader>(() =>
                {
                    IDataReader dataReader = null;
                    SqlDatabase db = CreateDatabase(connectionString);
                    var command = db.GetSqlStringCommand(sql);
                    command.CommandTimeout = sqlCommandTimeOut;
                    if (IsLogEnabled)
                    {
                        stopwatch.Value.Restart();
                        dataReader = CreateDatabase(connectionString).ExecuteReader(command);
                        stopwatch.Value.Stop();
                        logger.Value.TraceApi("SQL Database", "ADO.ExecuteFunction", stopwatch.Value.Elapsed, command);
                    }
                    else
                    {
                        dataReader = CreateDatabase(connectionString).ExecuteReader(command);
                    }
                    return dataReader;
                });
        }

        /// <summary>
        /// Processes a SQL query.
        /// </summary>
        /// <param name="connectionString">The connection string for the sql clause.</param>
        /// <param name="table">The table for the sql clause.</param>
        /// <returns>A IDataReader object.</returns>
        public static IDataReader GetTableReader(string connectionString, string table)
        {
            return ExecuteFunction(connectionString, "SELECT * FROM " + table);
        }

        /// <summary>
        /// Processes a SQL SELECT query that returns entire table.
        /// </summary>
        /// <param name="connectionString">The connection string for the sql clause.</param>
        /// <param name="sql">A sql query.</param>
        /// <returns>A IDataReader object.</returns>
        public static IDataReader ExecuteQuery(string connectionString, string sql)
        {
            return ExecuteFunction(connectionString, sql);
        }

        /// <summary>
        /// Processes a SQL query using a funtion.
        /// </summary>
        /// <param name="connectionString">The connection string for the sql clause.</param>
        /// <param name="function">The function to be executed.</param>
        /// <param name="parameters">The function parameters.</param>
        /// <returns>A IDataReader object.</returns>
        public static IDataReader ExecuteFunction(string connectionString, string function, params object[] parameters)
        {
            StringBuilder sb = new StringBuilder("SELECT * FROM " + function + "(");
            foreach (object param in parameters)
            {
                switch (param.GetType().Name)
                {
                    case "long":
                    case "int":
                        sb.Append(param + ",");
                        break;
                    case "string":
                        sb.Append("'");
                        sb.Append(param);
                        sb.Append("',");
                        break;
                }
                sb.Append(param + ",");
            }
            sb.Length = sb.Length - 1;
            sb.Append(")");
            return ExecuteFunction(connectionString, sb.ToString());
        }

        /// <summary>
        /// Processes a SQL query using a funtion.
        /// </summary>
        /// <param name="connectionString">The connection string for the sql clause.</param>
        /// <param name="function">The function to be executed.</param>
        /// <param name="parameters">The function parameters.</param>
        /// <returns>A IDataReader object.</returns>
        public static IDataReader ExecuteFunction(string connectionString, string function, params long[] parameters)
        {
            StringBuilder sb = new StringBuilder("SELECT * FROM " + function + "(");
            foreach (long param in parameters)
            {
                sb.Append(param + ",");
            }
            sb.Length = sb.Length - 1;
            sb.Append(") ");
            return ExecuteFunction(connectionString, sb.ToString());
        }

        /// <summary>
        /// Processes a SQL query using a funtion.
        /// </summary>
        /// <param name="connectionString">The connection string for the sql clause.</param>
        /// <param name="function">The function to be executed.</param>
        /// <param name="parameters">The function parameters.</param>
        /// <returns>A IDataReader object.</returns>
        public static IDataReader ExecuteFunction(string connectionString, string function, params string[] parameters)
        {
            StringBuilder sb = new StringBuilder("SELECT * FROM " + function + "(");
            foreach (string param in parameters)
            {
                sb.Append("'");
                sb.Append(param);
                sb.Append("',");
            }
            sb.Length = sb.Length - 1;
            sb.Append(")");
            return ExecuteFunction(connectionString, sb.ToString());
        }

        /// <summary>
        /// Executes a SQL store procedure.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="connectionString">The connection string for the sql clause.</param>
        /// <param name="storeProcedure">The store procedure.</param>
        /// <param name="parametersIn">The IN parameters.</param>
        /// <returns>A value of specified type.</returns>
        public static T ExecuteStoreProcedure<T>(string connectionString, string storeProcedure, Dictionary<string, object> parametersIn)
            where T : struct
        {
            return CoreExceptionProcessor.ProcessInfrastructure<T>(() =>
            {
                SqlDatabase db = CreateDatabase(connectionString);
                DbCommand command = db.GetStoredProcCommand(storeProcedure);
                foreach (var kvp in parametersIn)
                {
                    SqlParameter parameter = new SqlParameter();
                    parameter.Direction = ParameterDirection.Input;
                    parameter.ParameterName = kvp.Key;
                    parameter.Value = kvp.Value;
                    command.Parameters.Add(parameter);
                }
                SqlParameter parameterReturn = new SqlParameter();
                parameterReturn.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(parameterReturn);
                if (IsLogEnabled)
                {
                    stopwatch.Value.Restart();
                    db.ExecuteNonQuery(command);
                    stopwatch.Value.Stop();
                    logger.Value.TraceApi("SQL Database", "ADO.ExecuteStoreProcedure<T>", stopwatch.Value.Elapsed, command);
                }
                else
                {
                    db.ExecuteNonQuery(command);
                }
                return (T)parameterReturn.Value;
            });
        }

        /// <summary>
        /// Executes a SQL store procedure.
        /// </summary>
        /// <param name="connectionString">The connection string for the sql clause.</param>
        /// <param name="storeProcedure">The store procedure.</param>
        /// <param name="parameters">The IN parameters.</param>
        /// <returns>A IDataReader object.</returns>
        public static IDataReader ExecuteStoreProcedure(string connectionString, string storeProcedure, Dictionary<string, object> parameters)
        {
            return CoreExceptionProcessor.ProcessInfrastructure<IDataReader>(() =>
                {
                    IDataReader reader = null;

                    SqlDatabase db = CreateDatabase(connectionString);
                    DbCommand command = db.GetStoredProcCommand(storeProcedure);

                    foreach (var kvp in parameters)
                    {
                        SqlParameter parameter = new SqlParameter();
                        parameter.Direction = ParameterDirection.Input;
                        parameter.ParameterName = kvp.Key;
                        parameter.Value = kvp.Value;
                        command.Parameters.Add(parameter);
                    }

                    if (IsLogEnabled)
                    {
                        stopwatch.Value.Restart();
                        reader = db.ExecuteReader(command);
                        stopwatch.Value.Stop();
                        logger.Value.TraceApi("SQL Database", "ADO.ExecuteStoreProcedure", stopwatch.Value.Elapsed, command);
                    }
                    else
                    {
                        reader = db.ExecuteReader(command);
                    }

                    return reader;
                });
        }

        /// <summary>
        /// Processes a SQL query.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="function">The function to be executed.</param>
        /// <param name="parameters">The IN parameters.</param>
        /// <returns>A value of specified type.</returns>
        public static T ExecuteScalar<T>(string function, params long[] parameters)
        {
            return CoreExceptionProcessor.ProcessInfrastructure<T>(() =>
                {
                    StringBuilder sb = new StringBuilder("SELECT dbo.");
                    sb.Append(function);
                    sb.Append("(");

                    foreach (var param in parameters)
                    {
                        sb.Append(param);
                        sb.Append(",");
                    }
                    var command = DefaultDatabase.GetSqlStringCommand(sb.ToString());

                    T t;

                    if (IsLogEnabled)
                    {
                        stopwatch.Value.Restart();
                        t = (T)(DefaultDatabase.ExecuteScalar(command));
                        stopwatch.Value.Stop();
                        logger.Value.TraceApi("SQL Database", "ADO.ExecuteScalar<>", stopwatch.Value.Elapsed, command);
                    }
                    else
                    {
                        t = (T)(DefaultDatabase.ExecuteScalar(command));
                    }

                    return t;
                });
        }

        /// <summary>
        /// Returns the number of rows for a table.
        /// </summary>
        /// <param name="table">The table for the sql clause.</param>
        /// <param name="field">The field to count.</param>
        /// <returns>The number of rows.</returns>
        public static int ExecuteCount(string table, string field)
        {
            var command = DefaultDatabase.GetSqlStringCommand("SELECT COUNT(" + field + ") FROM dbo." + table);

            int c = -1;

            if (IsLogEnabled)
            {
                stopwatch.Value.Restart();
                c = (int)(DefaultDatabase.ExecuteScalar(command));
                stopwatch.Value.Stop();
                logger.Value.TraceApi("SQL Database", "ADO.ExecuteCount", stopwatch.Value.Elapsed, command);
            }
            else
            {
                c = (int)(DefaultDatabase.ExecuteScalar(command));
            }

            return c;
        }
        #endregion

        #region ·   Code First  ·
        /// <summary>
        /// Processes a SQL query using a funtion.
        /// </summary>
        /// <typeparam name="T">The object to be returned.</typeparam>
        /// <typeparam name="TContext">A DbContext object.</typeparam>
        /// <param name="sql">The sql query to be processed.</param>
        /// <param name="parameters">The SQL query parameters.</param>
        /// <returns>A List of T objects.</returns>
        public static IList<T> ExecuteFunction<T, TContext>(string sql, params long[] parameters)
            where TContext : DbContext, new()
        {
            return CoreExceptionProcessor.ProcessInfrastructure<IList<T>>(() =>
                {
                    StringBuilder sb = new StringBuilder("SELECT * FROM " + sql + "(");
                    foreach (long param in parameters)
                    {
                        sb.Append(param + ",");
                    }
                    sb.Length = sb.Length - 1;
                    sb.Append(") ");
                    using (TContext ctx = new TContext())
                    {
                        return ctx.Database.SqlQuery<T>(sql).ToList();
                    }
                });
        }

        /// <summary>
        /// Processes a SQL query using a funtion.
        /// </summary>
        /// <typeparam name="T">The object to be returned.</typeparam>
        /// <typeparam name="TContext">A DbContext object.</typeparam>
        /// <param name="sql">The sql query to be processed.</param>
        /// <param name="parameters">The SQL query parameters.</param>
        /// <returns>A List of T objects.</returns>
        public static IList<T> ExecuteFunction<T, TContext>(string sql, params string[] parameters)
            where TContext : DbContext, new()
        {
            return CoreExceptionProcessor.ProcessInfrastructure<IList<T>>(() =>
                {
                    StringBuilder sb = new StringBuilder("SELECT * FROM " + sql + "(");
                    foreach (string param in parameters)
                    {
                        sb.Append("'");
                        sb.Append(param);
                        sb.Append("',");
                    }
                    sb.Length = sb.Length - 1;
                    sb.Append(")");
                    using (TContext ctx = new TContext())
                    {
                        return ctx.Database.SqlQuery<T>(sql).ToList();
                    }
                });
        }

        /// <summary>
        /// Processes a SQL query.
        /// </summary>
        /// <typeparam name="T">The object to be returned.</typeparam>
        /// <typeparam name="TContext">A DbContext object.</typeparam>
        /// <param name="sql">The sql query to be processed.</param>
        /// <returns>A List of T objects.</returns>
        public static IList<T> ExecuteFunction<T, TContext>(string sql)
            where TContext : DbContext, new()
        {
            return CoreExceptionProcessor.ProcessInfrastructure<IList<T>>(() =>
                {
                    using (TContext ctx = new TContext())
                    {
                        return ctx.Database.SqlQuery<T>(sql).ToList();
                    }
                });
        }

        /// <summary>
        /// Processes a SQL query.
        /// </summary>
        /// <typeparam name="TContext">A DbContext derived type.</typeparam>
        /// <param name="sql">The sql query.</param>
        /// <param name="parameters">A list of parameters for the function.</param>
        /// <returns>The result of the executing query.</returns>
        public static int ExecuteScalar<TContext>(string sql, params object[] parameters)//TODO: context no se usa.
            where TContext : DbContext, new()
        {
            return CoreExceptionProcessor.ProcessInfrastructure<int>(() =>
                {
                    using (TContext ctx = new TContext())
                    {
                        return ctx.Database.ExecuteSqlCommand(sql, parameters);
                    }
                });
        }

        /// <summary>
        /// Processes a SQL query.
        /// </summary>
        /// <typeparam name="T">The object to be returned.</typeparam>
        /// <typeparam name="TContext">A DbContext object.</typeparam>
        /// <param name="sql">The sql query.</param>
        /// <param name="parameters">Parameters for the sql query.</param>
        /// <returns>The result of the executing query.</returns>
        public static IList<T> ExecuteQuery<T, TContext>(string sql, params object[] parameters)
    where TContext : DbContext, new()
        {
            return CoreExceptionProcessor.ProcessInfrastructure<IList<T>>(() =>
            {
                using (TContext ctx = new TContext())
                {
                    return ctx.Database.SqlQuery<T>(sql, parameters).ToList();
                }
            });
        }
        #endregion
    }
}
