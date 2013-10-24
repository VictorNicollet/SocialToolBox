namespace SocialToolBox.Core
{
    /// <summary>
    /// A pair of values.
    /// </summary>
    public static class Pair
    {
        /// <summary>
        /// Internal implementation.
        /// </summary>
        private sealed class Impl<TA, TB> : IPair<TA, TB>
        {
            public TA First { get; private set; }
            public TB Second { get; private set; }

            public Impl(TA first, TB second)
            {
                First = first;
                Second = second;
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;

                var asPair = obj as IPair<TA, TB>;
                if (asPair == null) return false;

                // ReSharper disable CompareNonConstrainedGenericWithNull
                if (First == null && asPair.First != null) return false;
                if (Second == null && asPair.Second != null) return false;

                return ((First == null || First.Equals(asPair.First))
                    && (Second == null || Second.Equals(asPair.Second)));
                // ReSharper restore CompareNonConstrainedGenericWithNull
            }

            public override int GetHashCode()
            {
                // ReSharper disable CompareNonConstrainedGenericWithNull
                var hasha = First == null ? 0 : First.GetHashCode();
                var hashb = Second == null ? 0 : Second.GetHashCode();
                // ReSharper restore CompareNonConstrainedGenericWithNull

                return hasha ^ hashb;
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
