using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.ReflectionModel;
using System.Linq;

namespace RaraAvis.nCubed.Core.Containers
{
    /// <summary>
    /// Container for serialize objects.
    /// </summary>
    public class SerializeContainer
    {
        private ConcurrentBag<Type> knownTypes = new ConcurrentBag<Type>();
        private CompositionContainer container = null;
        /// <summary>
        /// Base cosntructor.
        /// </summary>
        /// <param name="container">Container for discover types.</param>
        public SerializeContainer(CompositionContainer container)
        {
            this.container = container;
        }
        /// <summary>
        /// Known discovered types.
        /// </summary>
        public IEnumerable<Type> KnownTypes
        {
            get
            {
                return knownTypes;
            }
        }
        /// <summary>
        /// Searchs for types with custom metadata type based.
        /// </summary>
        /// <typeparam name="T">Type to search.</typeparam>
        /// <returns>This object.</returns>
        public SerializeContainer DiscoverCustomMetadataTypes<T>()
        {
            var cpds = (from cpd in container.Catalog
                        where cpd.Metadata.Any(x => String.Compare(x.Value.ToString(), typeof(T).FullName, StringComparison.OrdinalIgnoreCase) == 0 || String.Compare(x.Key.ToString(), typeof(T).FullName, StringComparison.OrdinalIgnoreCase) == 0)
                        select cpd).ToList();

            foreach (var v in cpds)
            {
                knownTypes.Add(ReflectionModelServices.GetPartType(v).Value);
            }
            return this;
        }
        private void DiscoverNonGenericKnownTypes(Type type)
        {
            var concretes = container.GetExports(type, null, null);

            foreach (var concrete in concretes)
            {
                Type t = concrete.Value.GetType();
                if (!knownTypes.Contains(t))
                {
                    knownTypes.Add(t);
                }
            }
        }
        private void DiscoverGenericKnownTypes<U>()
        {
            if (!knownTypes.Contains(typeof(U)))
            {
                var generic = container.GetExports(typeof(U), null, null).FirstOrDefault();

                if (generic != null)
                {
                    knownTypes.Add(generic.Value.GetType());
                }
            }
        }
        /// <summary>
        /// Search for a type inside loaded assemblies.
        /// </summary>
        /// <typeparam name="T">Type to search.</typeparam>
        /// <param name="element">Object with type.</param>
        /// <returns>This object.</returns>
        public SerializeContainer DiscoverKnownTypes<T>(T element)
        {
            DiscoverGenericKnownTypes<T>();
            DiscoverNonGenericKnownTypes(element.GetType());
            return this;
        }
        /// <summary>
        /// Search for a type inside loaded assemblies.
        /// </summary>
        /// <param name="type">Type to search.</param>
        /// <returns>This object.</returns>
        public SerializeContainer DiscoverKnownTypes(Type type)
        {
            DiscoverNonGenericKnownTypes(type);
            return this;
        }
    }
}
