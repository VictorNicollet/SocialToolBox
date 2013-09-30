using System.Threading.Tasks;
using SocialToolBox.Core.Database.EventStream;

namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// Processes events by updating an index.
    /// </summary>
    public interface IIndexProjection<TEv,out TSet,TSort>
        where TEv : class
        where TSet : class 
        where TSort : class
    {
        /// <summary>
        /// Process an event, possibly by updating a writable store.
        /// </summary>
        Task Process(IWritableIndex<TSet, TSort> index, EventInStream<TEv> ev, IProjectCursor cursor);
    }
}
