namespace SocialToolBox.Core.Web.Args
{
    /// <summary>
    /// When a type can be used as an arguments type.
    /// </summary>
    public interface IConvertibleToArgs<out TArgs> where TArgs : class, IWebUrlArgument
    {
        /// <summary>
        /// Returns the argument representation of this value.
        /// </summary>
        TArgs ToArgs();
    }
}
