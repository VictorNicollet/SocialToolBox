using System;

namespace SocialToolBox.Core.Database.Index
{
    /// <summary>
    /// Marks a class as being usable as an index key attribute, making it 
    /// usable as a parameter for index-based projections.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class IndexKeyAttribute : Attribute
    {
    }
}
