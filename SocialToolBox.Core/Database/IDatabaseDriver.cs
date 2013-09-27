namespace SocialToolBox.Core.Database
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

        /// <summary>
        /// A clock registry used by projection aspects of this database
        /// driver.
        /// </summary>
        IClockRegistry ClockRegistry { get; }

        /// <summary>
        /// The projection engine for this database drive.
        /// </summary>
        ProjectionEngine Projections { get; }

        /// <summary>
        /// Create a new projection using the specified name, that accepts
        /// the provided events.
        /// </summary>
        IProjection<TEv> CreateProjection<TEv>(string name) where TEv : class;

        /// <summary>
        /// Start a new read transaction compatible with all readable objects
        /// on this database driver.
        /// </summary>
        /// <remarks>
        /// Don't call this function unless you know what you're doing.
        /// </remarks>
        IReadCursor OpenReadCursor();

        /// <summary>
        /// Start a new read-write transaction compatible with all projection
        /// objects on this database driver, and which can read from streams.
        /// </summary>
        /// <remarks>
        /// Don't call this function unless you know what you're doing.
        /// </remarks>
        IProjectCursor OpenProjectionCursor();

        /// <summary>
        /// Starts a new read-write transaction that can read from all objects
        /// on this database server, and can write to streams.
        /// </summary>
        /// <remarks>
        /// Don't call this function unless you know what you're doing.
        /// </remarks>
        ICursor OpenReadWriteCursor();
    }
}
