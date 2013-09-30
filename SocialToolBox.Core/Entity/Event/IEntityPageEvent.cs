namespace SocialToolBox.Core.Entity.Event
{
    /// <summary>
    /// An event related to a page entity.
    /// </summary>
    public interface IEntityPageEvent : IEventWithEntityId
    {
        /// <summary>
        /// Returns true if this event changed the name of the entity page.
        /// </summary>
        bool EntityTitleChanged { get; }
    }
}
