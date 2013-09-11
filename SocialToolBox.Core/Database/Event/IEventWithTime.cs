using System;

namespace SocialToolBox.Core.Database.Event
{
    /// <summary>
    /// An event that carries time information.
    /// </summary>
    public interface IEventWithTime
    {        
        /// <summary>
        /// The time at which this event occured.
        /// </summary>
        DateTime Time { get; }
    }
}
