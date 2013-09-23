using System.Threading.Tasks;
using SocialToolBox.Core.Database.EventStream;

namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// Processes events by updating a store.
    /// </summary>
    public interface IStoreProjection<TEv,TEn>
        where TEv : class 
        where TEn : class
    {
        /// <summary>
        /// Process an event, possibly by updating a writable store.
        /// </summary>
        Task Process(IWritableStore<TEn> store, EventInStream<TEv> ev);
    }
}
