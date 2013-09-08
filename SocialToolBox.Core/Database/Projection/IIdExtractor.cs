namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// An object that extracts identifers from events.
    /// </summary>
    public interface IIdExtractor<in T> where T : class
    {
        /// <summary>
        /// Return the identifier related to this event, if any.
        /// </summary>
        Id? EventIdentifier(T ev);
    }
}
