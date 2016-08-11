using RaraAvis.nCubed.Core.Containers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RaraAvis.nCubed.Core.Serialization
{
    /// <summary>
    /// Interface for serializers that can read/write an object graph to a stream.
    /// </summary>    
    public interface ITextSerializer
    {
        /// <summary>
        /// Container to be serialized.
        /// </summary>
        SerializeContainer SerializeContainer { get; set; }
        /// <summary>
        /// Serializes an object graph to a text reader.
        /// </summary>
        /// <param name="graph">Object to serialize.</param>
        /// <param name="type">Type to serialize.</param>
        /// <returns>Serialized object.</returns>
        string Serialize(object graph, Type type);
        /// <summary>
        /// Serializes an object graph to a text reader.
        /// </summary>
        /// <typeparam name="T">Type to serialize.</typeparam>
        /// <param name="graph">Object to serialize.</param>
        /// <returns>Object serialized.</returns>
        string Serialize<T>(T graph);
        /// <summary>
        /// Deserializes an object graph from the specified text reader.
        /// </summary>
        /// <typeparam name="T">Type to deserialize.</typeparam>
        /// <param name="serialized">Object to deserialize.</param>
        /// <returns>Object deserialized.</returns>
        T Deserialize<T>(string serialized);
        /// <summary>
        /// Loads a Type into serializer
        /// </summary>
        /// <param name="type">Type to serialize.</param>
        void LoadType(Type type);
    }
}
