namespace SocialToolBox.Core.Database
{
    /// <summary>
    /// An object that reads events of type T and processes them. 
    /// </summary>
    public interface IEventReader<in T> where T : class
    {
        void Read(T ev);
    }
}
