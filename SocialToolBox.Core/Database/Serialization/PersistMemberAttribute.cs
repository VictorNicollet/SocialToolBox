using System;

namespace SocialToolBox.Core.Database.Serialization
{
    /// <summary>
    /// Used inside a persistent class to mark attributes which should be
    /// persisted, and how.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PersistMemberAttribute : Attribute
    {
        /// <summary>
        /// The order in which attributes are serialized and unserialized.
        /// </summary>
        public readonly int Order;

        /// <summary>
        /// Polymorphic fields include type information to instantiate
        /// the correct class. By default, this is not the case.
        /// </summary>
        public bool Polymorphic { get; set; }

        public PersistMemberAttribute(int order)
        {
            Order = order;
            Polymorphic = false;
        }
    }
}
