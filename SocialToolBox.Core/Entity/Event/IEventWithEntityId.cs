using SocialToolBox.Core.Database;

namespace SocialToolBox.Core.Entity.Event
{
    /// <summary>
    /// Events that are related to entities should carry an entity id.
    /// </summary>
    public interface IEventWithEntityId
    {
        Id EntityId { get; }
    }
}
