namespace SocialToolBox.Core.Database.Event
{
    public interface IEventWithId
    {
        /// <summary>
        /// The user to which this event applies.
        /// </summary>
        Id Id { get; }
    }
}
