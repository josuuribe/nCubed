using RaraAvis.nCubed.Core.Configurations;
using RaraAvis.nCubed.Core.Configurations.Common.Types;
using RaraAvis.nCubed.Core.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.ComponentModel.Composition.Primitives;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using RaraAvis.nCubed.Core.Exceptions.Core;
using RaraAvis.nCubed.Core.Exceptions;
using RaraAvis.nCubed.Core.Containers.DI;

namespace RaraAvis.nCubed.Core.Containers
{
    /// <summary>
    /// Base common container.
    /// </summary>
    /// <typeparam name="T">The serializer type.</typeparam>
    public class SerializerContainer<T> : SystemContainer where T : ITextSerializer, new()
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public SerializerContainer()
            : base()
        {
            DIManager.Registered += SerializerContainer_Registered;
        }
        private void SerializerContainer_Registered(object sender, RegisteredEventArgs e)
        {
            T serializerSimple = new T();

            serializerSimple.SerializeContainer = new SerializeContainer(e.ContainerSimple);

            (sender as DIManager).ContainerSimple.ComposeExportedValue<ITextSerializer>(serializerSimple);
        }
        /// <summary>
        /// Registers types configuration based.
        /// </summary>
        protected virtual void Register()
        {
            base.Register(AllTypes);
        }
    }
}
