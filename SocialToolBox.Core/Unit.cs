namespace SocialToolBox.Core
{
    /// <summary>
    /// A singleton class, used as a replacement for void in generics.
    /// </summary>
    public class Unit
    {
        public static Unit Instance;

        private Unit() {}

        static Unit()
        {
            Instance = new Unit();
        }
    }
}
