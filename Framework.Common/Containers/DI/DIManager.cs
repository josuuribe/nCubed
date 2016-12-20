using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.ComponentModel.Composition.Primitives;
using RaraAvis.nCubed.Core.Configurations.Common.Types;
using System.Reflection;
using RaraAvis.nCubed.Core.Exceptions.Core;
using RaraAvis.nCubed.Core.Properties;

namespace RaraAvis.nCubed.Core.Containers.DI
{
    /// <summary>
    /// A DIManager for manage object creation.
    /// </summary>
    public class DIManager : IDisposable
    {
        /// <summary>
        /// Gets a simple container.
        /// </summary>
        protected internal CompositionContainer ContainerSimple { get; set; }
        /// <summary>
        /// Gets a factory container, a container more complex with nested types.
        /// </summary>
        protected internal CompositionContainer ContainerFactory { get; set; }
        /// <summary>
        /// Registering event.
        /// </summary>
        public event EventHandler<RegisteringEventArgs> Registering;
        /// <summary>
        /// Registered event.
        /// </summary>
        public event EventHandler<RegisteredEventArgs> Registered;
        /// <summary>
        /// Registering method.
        /// </summary>
        /// <param name="e">Event raised.</param>
        private void OnRegistering(RegisteringEventArgs e)
        {
            if (Registering != null)
                Registering(this, e);
        }
        /// <summary>
        /// Registered method.
        /// </summary>
        /// <param name="e">Event raised.</param>
        private void OnRegistered(RegisteredEventArgs e)
        {
            if (Registered != null)
                Registered(this, e);
        }
        /// <summary>
        /// Method that read configuration and process all assemblies.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "MEF")]
        internal void Register(IList<Tuple<string, string>> pathsToScan)
        {
            RegisteringEventArgs rea = new RegisteringEventArgs();

            OnRegistering(rea);

            AggregateCatalog aggc = new AggregateCatalog();
            List<DirectoryCatalog> catalogs = new List<DirectoryCatalog>();

            List<ComposablePartDefinition> parts = new List<ComposablePartDefinition>();

            foreach (var pathScan in pathsToScan)
            {
                string folder = String.IsNullOrEmpty(pathScan.Item1) ? System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) : pathScan.Item1;//Default if empty
                string path = System.IO.Path.Combine(folder, pathScan.Item2);//Full path
                if (!catalogs.Exists(x => System.IO.Path.Combine(x.FullPath.ToUpperInvariant(), x.SearchPattern).ToUpperInvariant() == path.ToUpperInvariant()))
                {//Check if same path is already loaded
                    DirectoryCatalog dc = new DirectoryCatalog(folder, pathScan.Item2);
                    if (dc.Parts.All(x => parts.Exists(y => y.ToString() == x.ToString())))
                    {//Check all parts, same assembly with different name or same assembly in two differents locations
                        dc.Dispose();
                    }
                    else
                    {
                        parts.AddRange(dc.Parts);
                        if (dc.LoadedFiles.Count() > 0)
                        {//Only add if assemblies loaded
                            catalogs.Add(dc);
                            for (int i = 0; i < catalogs.IndexOf(dc); i++)
                            {
                                if (catalogs.ElementAt(i).LoadedFiles.Intersect(dc.LoadedFiles).Count() > 0)
                                {//There are similar files
                                    if (catalogs.ElementAt(i).LoadedFiles.Count() > dc.LoadedFiles.Count())
                                    {//One of them could have more files than another, take the higher
                                        catalogs.Remove(dc);
                                    }
                                    else
                                    {
                                        catalogs.Remove(catalogs.ElementAt(i));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            catalogs.ForEach(x => aggc.Catalogs.Add(x));

            aggc.Catalogs.Add(rea.TypeCatalog);

            ContainerSimple = new CompositionContainer(aggc, CompositionOptions.DisableSilentRejection | CompositionOptions.IsThreadSafe);

            var scopeDefDependent = new CompositionScopeDefinition(ContainerSimple.Catalog, null);

            var appCatalog = new TypeCatalog(typeof(ApplicationFactory<>));

            var scopeDefRoot = new CompositionScopeDefinition(appCatalog, new[] { scopeDefDependent });

            ContainerFactory = new CompositionContainer(scopeDefRoot, CompositionOptions.IsThreadSafe);

            OnRegistered(new RegisteredEventArgs(this.ContainerSimple, this.ContainerFactory));
        }
        /// <summary>
        /// Create the specified object or default if can not be constructed.
        /// </summary>
        /// <typeparam name="T">The type to the object to be created.</typeparam>
        /// <returns>An object to be specified.</returns>
        public T CreateObject<T>() where T : class
        {
            return CoreExceptionProcessor.ProcessCore<T>(
                () => ContainerSimple.GetExportedValueOrDefault<T>()
            );
        }
        /// <summary>
        /// Creates the specified object or default if can not be constructed.
        /// </summary>
        /// <typeparam name="T">The type to the object to be created.</typeparam>
        /// <param name="contractName">The MEF contract name.</param>
        /// <returns>Created object.</returns>
        public T CreateObject<T>(string contractName) where T : class
        {
            return CoreExceptionProcessor.ProcessCore<T>(
                () => ContainerSimple.GetExportedValueOrDefault<T>(contractName)
            );
        }
        /// <summary>
        /// Creates the specified object or throws an error if not possible.
        /// </summary>
        /// <typeparam name="T">The type to the object to be created.</typeparam>
        /// <returns>An object to be specified.</returns>
        public T CreateRequiredObject<T>() where T : class
        {
            return CoreExceptionProcessor.ProcessCore<T>(
                () =>
                {
                    try
                    {
                        return ContainerSimple.GetExportedValue<T>();
                    }
                    catch (ImportCardinalityMismatchException icme)
                    {
                        throw new System.ArgumentNullException(String.Format(System.Globalization.CultureInfo.InvariantCulture, Resources.ExceptionCanNotCreateObject, icme.Message), icme);
                    }
                }
            );
        }
        /// <summary>
        /// Creates the specified object or throws an error if not possible.
        /// </summary>
        /// <typeparam name="T">The type to the object to be created.</typeparam>
        /// <param name="contractName">The MEF contract name.</param>
        /// <returns>An object to be specified.</returns>
        public T CreateRequiredObject<T>(string contractName) where T : class
        {
            return CoreExceptionProcessor.ProcessCore<T>(
                () =>
                {
                    try
                    {
                        return ContainerSimple.GetExportedValue<T>(contractName);
                    }
                    catch (ImportCardinalityMismatchException icme)
                    {
                        throw new System.ArgumentNullException(String.Format(System.Globalization.CultureInfo.InvariantCulture, Resources.ExceptionCanNotCreateObject, icme.Message), icme);
                    }
                }
            );
        }
        /// <summary>
        /// Creates A factory por manage the object, it is required if uses DbContext.
        /// </summary>
        /// <typeparam name="T">The type to the object to be created.</typeparam>
        /// <returns>A FactoryManager object.</returns>
        public FactoryManager<T> CreateWideFactory<T>() where T : IDisposable
        {
            return CoreExceptionProcessor.ProcessCore<FactoryManager<T>>(
                    () => new FactoryManager<T>(this.ContainerFactory)
                );
        }
        /// <summary>
        /// Get all types for a typen given.
        /// </summary>
        /// <typeparam name="T">Type to search for.</typeparam>
        /// <returns>A list of T type objects.</returns>
        public IEnumerable<Lazy<T>> GetExports<T>()
        {
            return CoreExceptionProcessor.ProcessCore<IEnumerable<Lazy<T>>>(
                    () => ContainerSimple.GetExports<T>()
                );
        }
        /// <summary>
        /// Get all types for a typen given given a contract name.
        /// </summary>
        /// <typeparam name="T">Type to search for.</typeparam>
        /// <param name="contractName">Contract name to search for.</param>
        /// <returns>A list of T type objects.</returns>
        public IEnumerable<Lazy<T>> GetExports<T>(string contractName)
        {
            return CoreExceptionProcessor.ProcessCore<IEnumerable<Lazy<T>>>(
                    () => ContainerSimple.GetExports<T>(contractName)
                );
        }
        /// <summary>
        /// Dispose object releasing all containers, <see cref="M:System.IDisposable.Dispose"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        /// <param name="disposing">False if unmanaged resources must be disposed, false otherwise.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ContainerSimple.Dispose();
                ContainerFactory.Dispose();
            }
        }
    }
}
