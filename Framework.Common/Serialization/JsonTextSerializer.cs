using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ComponentModel.Composition;
using System.IO;
using System.Xml;
using System.Configuration;
using RaraAvis.nCubed.Core.Containers;
using System.ComponentModel.Composition.Hosting;

namespace RaraAvis.nCubed.Core.Serialization
{
    /// <summary>
    /// Class that serializes Json based.
    /// </summary>
    public class JsonTextSerializer : ITextSerializer
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private static string Serialize<T>(T graph, IEnumerable<Type> knownTypes)
        {
            byte[] read;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T), knownTypes);
            using (var ms = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(ms))
                {
                    serializer.WriteObject(ms, (T)graph);
                    read = new byte[ms.Length];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(read, 0, (int)ms.Length);
                }
            }
            return Encoding.UTF8.GetString(read);
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
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T), knownTypes);
                return (T)serializer.ReadObject(ms);
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
