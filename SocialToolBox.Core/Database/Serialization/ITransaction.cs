namespace SocialToolBox.Core.Database.Serialization
{
    /// <summary>
    /// A read-write transaction used by the query side. It cannot write to 
    /// projections, but it can push events.
    /// </summary>
    public interface ITransaction : IReadTransaction
    {
    }
}
