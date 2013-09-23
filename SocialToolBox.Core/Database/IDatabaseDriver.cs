using System.Diagnostics;
using SocialToolBox.Core.Database.Projection;

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
        /// A factory for creating entity stores.
        /// </summary>
        IEntityStoreFactory EntityStore { get; }

        /// <summary>
        /// A factory for creating entity indexes.
        /// </summary>
        IEntityIndexFactory EntityIndex { get; }

        /// <summary>
        /// The projection engine for this database drive.
        /// </summary>
        ProjectionEngine Projections { get; }

        /// <summary>
        /// Create a new projection using the specified name, that accepts
        /// the provided events.
        /// </summary>
        IProjection<TEv> CreateProjection<TEv>(string name) where TEv : class;
    }
}
