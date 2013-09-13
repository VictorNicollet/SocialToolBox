using System;

namespace SocialToolBox.Core.Database.Index
{
    /// <summary>
    /// Index fields are used for sorting values in an index.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IndexFieldAttribute : Attribute
    {
        /// <summary>
        /// The rank of the field within the index key.
        /// </summary>
        public readonly int Order;

        /// <summary>
        /// Is this attribute allowed to be NULL ?
        /// </summary>
        public bool NotNull { get; set; }

        /// <summary>
        /// The maximum length (if this is a variable-length field).
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Perform a case-sensitive comparison ? Default is true.
        /// </summary>
        public bool IsCaseSensitive { get; set; }

        public IndexFieldAttribute(int order)
        {
            Order = order;
            NotNull = false;
            Length = 255;
            IsCaseSensitive = true;
        }
    }
}
