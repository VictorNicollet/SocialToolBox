namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// As part of a database driver, creates entity indexes: returns the
    /// <see cref="IEntityIndex{TSetKey,TSortKey,TEntity}"/> and registers 
    /// the <see cref="IProjector{T}"/>.
    /// </summary>
    public interface IEntityIndexFactory
    {
        /// <summary>
        /// The driver with which any projectors are registered.
        /// </summary>
        IDatabaseDriver Driver { get; }

        /// <summary>
        /// Create a new entity index using a projection, register the projector with
        /// the database driver.
        /// </summary>
        IEntityIndex<TSet,TSort,TEn> Create<TEv, TEn, TSet, TSort>(
            string name, IEntityIndexProjection<TEv, TEn, TSet, TSort> proj, IEventStream[] streams)
            where TEv : class
            where TEn : class
            where TSet : class
            where TSort : class;
    }
}
