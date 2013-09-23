using SocialToolBox.Core.Database.Projection;

namespace SocialToolBox.Core.Database
{
    /// <summary>
    /// A given projection allows the system to combine several 
    /// projectors into one, and to update several views at once
    /// from an individual event. It also allows cross-view 
    /// queries within the same projection.
    /// </summary>
    public interface IProjection<T> where T : class
    {
        /// <summary>
        /// The name of the projection, as defined when it was created.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Create an entity store using the specified projection.
        /// </summary>
        IStore<TEn> Create<TEn>(string name, IStoreProjection<T, TEn> proj,
            IEventStream[] streams)
            where TEn : class;

        /// <summary>
        /// Compiles the projection. This means using the projection contents
        /// becomes allowed, but adding new views or projectors becomes 
        /// forbidden. 
        /// </summary>
        void Compile();
    }
}
