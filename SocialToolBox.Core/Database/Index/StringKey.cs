namespace SocialToolBox.Core.Database.Index
{
    /// <summary>
    /// A one-column, 255-character string key. Provided as a helpful utility class.
    /// </summary>
    [IndexKey]
    public class StringKey
    {
        [IndexField(0)] public readonly string Key;

        public StringKey(string key) { Key = key; }

        public override string ToString()
        {
            return Key;
        }
    }
}
