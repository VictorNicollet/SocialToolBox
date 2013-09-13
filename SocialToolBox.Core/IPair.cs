namespace SocialToolBox.Core
{
    /// <summary>
    /// A generic pair interface.
    /// </summary>
    public interface IPair<out TA, out TB>
    {
        TA First { get; }
        TB Second { get; }
    }
}
