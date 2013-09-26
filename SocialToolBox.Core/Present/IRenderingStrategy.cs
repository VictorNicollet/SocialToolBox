namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// A rendering strategy picks a renderer based on some parameters.
    /// </summary>
    public interface IRenderingStrategy<in T>
    {
        INodeRenderer PickRenderer(T parameters);
    }
}
