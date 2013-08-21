﻿namespace SocialToolBox.Core.Database
{
    /// <summary>
    /// Database drivers describe how data can be written to and
    /// retrieved from a database.
    /// </summary>
    /// <remarks>
    /// The interface is intentionally restrictive to make 
    /// reimplementation easier on different platforms. 
    /// </remarks>
    public interface IDatabaseDriver
    {
        /// <summary>
        /// Retrieve a reference to an event stream from the database, 
        /// based on its name, <code>null</code> if it does not exist. 
        /// </summary>
        IEventStream GetEventStream(string name, bool createIfMissing);

        /// <summary>
        /// A type dictionary used by all the components of this database
        /// driver.
        /// </summary>
        ITypeDictionary TypeDictionary { get; }
    }
}
