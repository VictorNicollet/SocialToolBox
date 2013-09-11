using System;
using System.IO;

namespace SocialToolBox.Core.Database.Serialization
{
    /// <summary>
    /// Defines functions for serializing and deserializing a generic or normal type.
    /// </summary>
    public interface ICustomSerializer
    {
        /// <summary>
        /// Serializes a value of the generic type. 
        /// </summary>
        void Serialize(UntypedSerializer serializer, object serialized, Stream output, Type[] typeargs = null);

        /// <summary>
        /// Unserializes a value of the generic type. 
        /// </summary>
        object Unserialize(UntypedSerializer serializer, Stream input, Type[] typeargs = null);
    }
}
