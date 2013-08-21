using System;

namespace SocialToolBox.Core.Database
{
    /// <summary>
    /// A type dictionary maps serializable types to integer 
    /// identifiers. This map should always be reflexive and
    /// deterministic.
    /// </summary>
    public interface ITypeDictionary
    {
        /// <summary>
        /// Retrieves the integer associated with a serializable type
        /// in this dictionary. 
        /// </summary>
        int FindIdentifier(Type t);

        /// <summary>
        /// Retrieves the type associated with an identifier.
        /// </summary>
        Type FindType(int id);

        /// <summary>
        /// For performance reasons, make sure the provided list of types
        /// has an associated identifier ahead of time. 
        /// </summary>
        void AssignIdentifiers(Type[] types);
    }
}
