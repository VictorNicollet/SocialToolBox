using SocialToolBox.Core.Database.Event;
using SocialToolBox.Core.Mocks.Database.Serialization;

namespace SocialToolBox.Core.Mocks.Database.Events
{
    /// <summary>
    /// An event related to a <see cref="MockAccount"/>
    /// </summary>
    public interface IMockEvent : IEventWithId, IEventWithTime
    {

    }
}
