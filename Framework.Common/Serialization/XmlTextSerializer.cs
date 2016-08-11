using RaraAvis.nCubed.Core.Containers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RaraAvis.nCubed.Core.Serialization
{
    /// <summary>
    /// Class that serializes Xml based.
    /// </summary>
    public class XmlTextSerializer : ITextSerializer
    {
        /// <summary>
        /// Container that stores serializing types.
        /// </summary>
        public SerializeContainer SerializeContainer
        {
            get;
            set;
        }
        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T">Type to serialize.</typeparam>
        /// <param name="graph">Objet to serialize.</param>
        /// <param name="knownTypes">List know types required to serialize.</param>
        /// <returns>Object serialized.</returns>
        private static string Serialize<T>(T graph, IEnumerable<Type> knownTypes)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), knownTypes.ToArray());
            using (StringWriter writer = new StringWriter(System.Globalization.CultureInfo.InvariantCulture))
            {
                serializer.Serialize(writer, graph);
                return writer.ToString();
            }
        }
        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <param name="graph">Objet to serialize.</param>
        /// <param name="type">Used to discover types required to serialize.</param>
        /// <returns>Object serialized.</returns>
        public string Serialize(object graph, Type type)
        {
            try
            {
                return Serialize(graph, SerializeContainer.KnownTypes);
            }

            catch (SerializationException)
            {
                return Serialize(graph, SerializeContainer.DiscoverKnownTypes(type).KnownTypes);
            }
        }
        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T">Type to serialize.</typeparam>
        /// <param name="graph">Objet to serialize.</param>
        /// <returns>Object serialized.</returns>
        public string Serialize<T>(T graph)
        {
            try
            {
                return Serialize<T>(graph, SerializeContainer.KnownTypes);
            }
            catch (SerializationException)
            {
                return Serialize<T>(graph, SerializeContainer.DiscoverKnownTypes<T>(graph).KnownTypes);
            }
        }
        /// <summary>
        /// Deserializes an object.
        /// </summary>
        /// <typeparam name="T">Type to deserialize</typeparam>
        /// <param name="serialized">Object serialized.</param>
        /// <param name="knownTypes">List know types required to serialize.</param>
        /// <returns>An object.</returns>
        private static T Deserialize<T>(string serialized, IEnumerable<Type> knownTypes)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(serialized);

            using (var ms = new MemoryStream(byteArray))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T), knownTypes.ToArray());
                return (T)serializer.Deserialize(ms);
            }
        }
        /// <summary>
        /// Deserializes an object.
        /// </summary>
        /// <typeparam name="T">Type to deserialize</typeparam>
        /// <param name="serialized">Object serialized.</param>
        /// <returns>An object.</returns>
        public T Deserialize<T>(string serialized)
        {
            try
            {
                return Deserialize<T>(serialized, SerializeContainer.KnownTypes);
            }
            catch (SerializationException)
            {
                return Deserialize<T>(serialized, SerializeContainer.DiscoverCustomMetadataTypes<T>().KnownTypes);
            }
        }
        /// <summary>
        /// Loads a type into known types.
        /// </summary>
        /// <param name="type">Type to discover known types.</param>
        public void LoadType(Type type)
        {
            SerializeContainer.DiscoverKnownTypes(type);
        }
    }
}
