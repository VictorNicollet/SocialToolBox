namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// As part of a database driver, creates entity stores: returns the
    /// <see cref="IEntityStore{T}"/> and registers the <see cref="IProjector{T}"/>.
    /// </summary>
    public interface IEntityStoreFactory
    {
        /// <summary>
        /// The driver with which any projectors are registered.
        /// </summary>
        IDatabaseDriver Driver { get; }

        /// <summary>
        /// Create a new entity store using a projection, register the projector with
        /// the database driver.
        /// </summary>
        IEntityStore<TEn> Create<TEv, TEn>(string name, IEntityStoreProjection<TEv, TEn> proj, IEventStream[] streams)
            where TEv : class where TEn : class;
    }
}
