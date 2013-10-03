namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// When a value bound to an identifier changes during a 
    /// projection update.
    /// </summary>
    public delegate void ValueChangedEvent<T>(ValueChangedEventArgs<T> args);
}
