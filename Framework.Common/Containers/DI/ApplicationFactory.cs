using RaraAvis.nCubed.Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Containers.DI
{
    /// <summary>
    /// Creates an application factory.
    /// </summary>
    /// <typeparam name="T">Type to create.</typeparam>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ApplicationFactory<T> : IDisposable where T : IDisposable
    {
        private ExportFactory<T> exportFactory;
        private Stack<ExportLifetimeContext<T>> stack;
        /// <summary>
        /// Constructor for factory.
        /// </summary>
        /// <param name="exportFactory">Exports a factory.</param>
        [ImportingConstructor]
        public ApplicationFactory([Import] ExportFactory<T> exportFactory)
        {
            stack = new Stack<ExportLifetimeContext<T>>();
            this.exportFactory = exportFactory;
        }
        /// <summary>
        /// Create an object.
        /// </summary>
        public void CreateObject()
        {
            var lifeOfA = exportFactory.CreateExport();
            stack.Push(lifeOfA);
        }
        /// <summary>
        /// Gets the value previously created.
        /// </summary>
        public T Value
        {
            get
            {
                return stack.Peek().Value;
            }
        }
        /// <summary>
        /// Destroys the last object created.
        /// </summary>
        public void DestroyLastObject()
        {
            DestroyObject();
        }
        private void DestroyObject()
        {
            try
            {
                var life = stack.Pop();
                life.Value.Dispose();
                life.Dispose();
                life = null;
            }
            catch (InvalidOperationException ioe)
            {
                throw new System.ArgumentNullException(Resources.ExceptionDestroyNonExistingObject, ioe);
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
                while (stack.Count > 0)
                {
                    DestroyObject();
                }
            }
            disposed = true;
        }
        #endregion
    }
}