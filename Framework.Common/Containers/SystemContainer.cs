using RaraAvis.nCubed.Core.Configurations.Common.Types;
using RaraAvis.nCubed.Core.Containers.DI;
using System;
using System.Collections.Generic;

namespace RaraAvis.nCubed.Core.Containers
{
    /// <summary>
    /// Container that reads system configuration.
    /// </summary>
    public class SystemContainer : IDisposable
    {
        /// <summary>
        /// DIManager for access types.
        /// </summary>
        protected DIManager DIManager { get; private set; }
        /// <summary>
        /// Base constructor.
        /// </summary>
        public SystemContainer()
        {
            DIManager = new DIManager();
        }
        /// <summary>
        /// Register types configuration based.
        /// </summary>
        /// <param name="allTypes">Register all types defined.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Error is intentional")]
        protected void Register(IEnumerable<TypesElement> allTypes)
        {
            List<Tuple<string, string>> types = new List<Tuple<string, string>>();
            foreach (TypesElement configurationElement in allTypes)
            {
                foreach (TypeElement pathElement in configurationElement.Types)
                {
                    types.Add(new Tuple<string, string>(pathElement.Path, pathElement.File));
                }
            }
            DIManager.Register(types);
        }
        /// <summary>
        /// Types for module given.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "It is for inherited classes")]
        protected virtual IEnumerable<TypesElement> AllTypes
        {
            get;
            private set;
        }
        #region ·   IDisposable Members ·
        bool disposed = false;
        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
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
            if (!disposed)
            {
                DIManager.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
}
