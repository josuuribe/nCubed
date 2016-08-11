using RaraAvis.nCubed.Core.Infrastructure;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace RaraAvis.nCubed.Core.Infrastructure.Sql
{
    /// <summary>
    /// Connection factory.
    /// </summary>
    internal class ServiceConfigurationSettingConnectionFactory : IDbConnectionFactory
    {
        private readonly object lockObject = new object();
        private readonly IDbConnectionFactory parent;
        private Dictionary<string, string> cachedConnectionStringsMap = new Dictionary<string, string>();
        /// <summary>
        /// Base constructor.
        /// </summary>
        public ServiceConfigurationSettingConnectionFactory()
        {
            this.parent = new SqlConnectionFactory();
        }
        /// <summary>
        /// Creates a connection.
        /// </summary>
        /// <param name="nameOrConnectionString">Connection string name.</param>
        /// <returns>a DbConnection.</returns>
        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            string connectionString = null;
            if (!IsConnectionString(nameOrConnectionString))
            {
                if (!this.cachedConnectionStringsMap.TryGetValue(nameOrConnectionString, out connectionString))
                {
                    lock (this.lockObject)
                    {
                        if (!this.cachedConnectionStringsMap.TryGetValue(nameOrConnectionString, out connectionString))
                        {
                            var connectionStringName = "DbContext." + nameOrConnectionString;
                            //TODO: Poner cadena correcta
                            //var settingValue = CloudConfigurationManager.GetSetting(connectionStringName);
                            var settingValue = "Cadena de conexión";
                            if (!string.IsNullOrEmpty(settingValue))
                            {
                                connectionString = settingValue;
                            }

                            if (connectionString == null)
                            {
                                try
                                {
                                    var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
                                    if (connectionStringSettings != null)
                                    {
                                        connectionString = connectionStringSettings.ConnectionString;
                                    }
                                }
                                catch (ConfigurationErrorsException)
                                {
                                }
                            }

                            var immutableDictionary = this.cachedConnectionStringsMap
                                .Concat(new[] { new KeyValuePair<string, string>(nameOrConnectionString, connectionString) })
                                .ToDictionary(x => x.Key, x => x.Value);

                            this.cachedConnectionStringsMap = immutableDictionary;
                        }
                    }
                }
            }

            if (connectionString == null)
            {
                connectionString = nameOrConnectionString;
            }

            return this.parent.CreateConnection(connectionString);
        }

        private static bool IsConnectionString(string connectionStringCandidate)
        {
            return (connectionStringCandidate.IndexOf('=') >= 0);
        }
    }


}
