using System;

namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// Useful extensions to the entity factory store.
    /// </summary>
    public static class EntityStoreFactoryExtensions
    {
        /// <summary>
        /// Implements an entity store projection using a reader.
        /// </summary>
        private class WithReader<TEv, TEn> : IEntityStoreProjection<TEv, TEn> 
            where TEn : class, IEventReader<TEv>, new()
            where TEv : class
        {
            private readonly Func<TEv, Id?> _getId;

            public WithReader(Func<TEv, Id?> getId)
            {
                _getId = getId;
            }

            public Id? EventIdentifier(TEv ev)
            {
                return _getId(ev);
            }

            public TEn Update(Id id, TEv ev, TEn old)
            {
                if (old == null) old = new TEn();
                old.Read(ev);
                return old;
            }
        }

        /// <summary>
        /// Implements an entity store using a <see cref="IEventReader{T}"/> type.
        /// </summary>
        public static IEntityStore<TEn> Create<TEv, TEn>(this IEntityStoreFactory factory,
            string name, Func<TEv, Id?> id, IEventStream[] streams) 
            where TEn : class, IEventReader<TEv>, new() 
            where TEv : class
        {
            var proj = new WithReader<TEv, TEn>(id);
            return factory.Create(name, proj, streams);
        }
    }
}
