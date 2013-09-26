namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// A rendering strategy that always returns the same renderer regardless
    /// of parameters.
    /// </summary>
    public class NaiveRenderingStrategy<T> : IRenderingStrategy<T>
    {
        /// <summary>
        /// The renderer that is returned regardless of parameters.
        /// </summary>
        public readonly INodeRenderer Renderer;

        public NaiveRenderingStrategy(INodeRenderer renderer)
        {
            Renderer = renderer;
        }

        /// <summary>
        /// Returns <see cref="Renderer"/>
        /// </summary>
        public INodeRenderer PickRenderer(T parameters)
        {
            return Renderer;
        }
    }
}
