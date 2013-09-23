namespace SocialToolBox.Core.Entity.Projection
{
    /// <summary>
    /// A view of an entity as something that can be displayed on 
    /// its own page. Should be persistable.
    /// </summary>
    public interface IEntityPage
    {
        /// <summary>
        /// The title of this entity when displayed as a page.
        /// </summary>
        string Title { get; }   
    }
}
