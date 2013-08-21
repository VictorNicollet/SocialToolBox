using System;

namespace SocialToolBox.Core.Database.Serialization
{
    /// <summary>
    /// While unserializing an object, a type identifier could not be associated
    /// with an actual type.
    /// </summary>
    class UnknownTypeIdException : Exception
    {
        /// <summary>
        /// The unbound type identifier.
        /// </summary>
        public readonly int TypeId;

        /// <summary>
        /// The dictionary in which the type identifier could not be
        /// found.
        /// </summary>
        public readonly ITypeDictionary Dictionary;

        public UnknownTypeIdException(int typeId, ITypeDictionary dictionary) 
            : base("Type identifier is not bound to a type.")
        {
            TypeId = typeId;
            Dictionary = dictionary;
        }
    }
}
