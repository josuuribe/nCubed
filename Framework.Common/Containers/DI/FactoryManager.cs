using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Containers.DI
{
    /// <summary>
    /// Class that creates factories.
    /// </summary>
    /// <typeparam name="T">Object to create.</typeparam>
    public class FactoryManager<T> : IDisposable where T : IDisposable
    {
        private CompositionContainer compositionContainer;
        private Lazy<ApplicationFactory<T>> application;
        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="compositionContainer">Container that stores all types.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Error is intentional")]
        public FactoryManager(CompositionContainer compositionContainer)
        {
            this.compositionContainer = compositionContainer;
            application = compositionContainer.GetExport<ApplicationFactory<T>>();
            application.Value.CreateObject();
        }
        /// <summary>
        /// The factory created.
        /// </summary>
        public ApplicationFactory<T> Factory
        {
            get
            {
                return application.Value;
            }
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
                compositionContainer.ReleaseExport(application);
                application = null;
            }
            disposed = true;
        }
        #endregion
    }
}
