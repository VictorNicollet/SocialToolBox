using System.Threading.Tasks;
using SocialToolBox.Core.Database.EventStream;

namespace SocialToolBox.Core.Database
{
    /// <summary>
    /// A clock registry stores (persistently) the vector clocks for
    /// a group of named projections.
    /// </summary>
    public interface IClockRegistry
    {
        /// <summary>
        /// Load the vector clock for the named projection. If no vector clock is 
        /// currently available, create a new one.
        /// </summary>
        Task<VectorClock> LoadProjection(string name);

        /// <summary>
        /// Save the vector clock for the named projection.
        /// </summary>
        /// <remarks>
        /// If the projection performs non-idempotent operations, then
        /// this function should be called as part of the same transaction
        /// as those operations.
        /// </remarks>
        Task SaveProjection(string name, VectorClock clock);
    }
}
