using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// Provides write-access to an index.
    /// </summary>
    public interface IWritableIndex<in TSet, TSort> : IIndex<TSet, TSort>
        where TSet : class where TSort : class
    {
        /// <summary>
        /// Delete all index bindings for the specified identifier.
        /// </summary>
        Task Delete(Id id, IProjectCursor c);

        /// <summary>
        /// Deletes a specified set for the specified identifier.
        /// </summary>
        Task DeleteSet(Id id, TSet set, IProjectCursor c);

        /// <summary>
        /// Adds a binding for a given identifier.
        /// </summary>
        Task Add(Id id, TSet set, TSort sort, IProjectCursor c);

        /// <summary>
        /// Adds several bindings for a given identifier.
        /// </summary>
        Task Add(Id id, IEnumerable<IPair<TSet, TSort>> bindings, IProjectCursor c);

        /// <summary>
        /// Updates or creates the single binding for a given identifier. If
        /// multiple bindings were already present, one or more may be updated.
        /// </summary>
        Task Set(Id id, TSet set, TSort sort, IProjectCursor c);
    }
}
