using System;
using SocialToolBox.Core.Database;

namespace SocialToolBox.Core.User.Event
{
    /// <summary>
    /// Events concerning the user.
    /// </summary>
    public interface IUserEvent
    {
        /// <summary>
        /// The user to which this event applies.
        /// </summary>
        Id User { get; }

        /// <summary>
        /// The time at which this event occured.
        /// </summary>
        DateTime Time { get; }
    }
}
