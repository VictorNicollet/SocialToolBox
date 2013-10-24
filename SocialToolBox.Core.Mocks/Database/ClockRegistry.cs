using System.Collections.Generic;
using System.Threading.Tasks;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.EventStream;

namespace SocialToolBox.Core.Mocks.Database
{
    public class ClockRegistry : IClockRegistry
    {
        /// <summary>
        /// A mutable, internal projection-to-clock mapping.
        /// </summary>
        public readonly Dictionary<string, VectorClock> Clocks;

        public ClockRegistry()
        {
            Clocks = new Dictionary<string, VectorClock>();
        }
 
        // ReSharper disable CSharpWarnings::CS1998
        public async Task<VectorClock> LoadProjection(string name)
        // ReSharper restore CSharpWarnings::CS1998
        {
            VectorClock clock;
            if (!Clocks.TryGetValue(name, out clock)) clock = new VectorClock();
            return clock.Clone();
        }

        // ReSharper disable CSharpWarnings::CS1998
        public async Task SaveProjection(string name, VectorClock clock)
        // ReSharper restore CSharpWarnings::CS1998
        {
            Clocks.Remove(name);
            Clocks.Add(name, clock.Clone());
        }
    }
}
