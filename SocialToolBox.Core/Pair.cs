using System.Runtime.CompilerServices;

namespace SocialToolBox.Core
{
    /// <summary>
    /// A pair of values.
    /// </summary>
    public class Pair
    {
        /// <summary>
        /// Internal implementation.
        /// </summary>
        private class Impl<TA, TB> : IPair<TA, TB>
        {
            public TA First { get; private set; }
            public TB Second { get; private set; }

            public Impl(TA first, TB second)
            {
                First = first;
                Second = second;
            }
        }

        /// <summary>
        /// Make a new pair.
        /// </summary>
        public static IPair<TA, TB> Make<TA, TB>(TA first, TB second)
        {
            return new Impl<TA, TB>(first, second);
        }
    }
}
