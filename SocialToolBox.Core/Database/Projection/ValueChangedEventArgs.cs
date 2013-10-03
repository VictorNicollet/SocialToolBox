namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// The arguments for <see cref="ValueChangedEvent{T}"/>
    /// </summary>
    public class ValueChangedEventArgs<T>
    {
        /// <summary>
        /// The identifier for which the value changed.
        /// </summary>
        public readonly Id Id;

        /// <summary>
        /// The old value bound to the identifier. Possibly null
        /// if this is a new value.
        /// </summary>
        public readonly T OldValue;

        /// <summary>
        /// The new (current) value bound to the identifier. Possibly
        /// null if the value was deleted.
        /// </summary>
        public readonly T NewValue;

        /// <summary>
        /// The cursor through which the change was performed.
        /// </summary>
        public readonly IProjectCursor Cursor;

        public ValueChangedEventArgs(Id id, T oldValue, T newValue, IProjectCursor cursor)
        {
            Id = id;
            OldValue = oldValue;
            NewValue = newValue;
            Cursor = cursor;
        }
    }
}
