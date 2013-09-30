using System;

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

        public override bool Equals(object obj)
        {
            var sk = obj as StringKey;
            if (sk == null) return false;
            return string.Compare(Key, sk.Key, StringComparison.InvariantCulture) == 0;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}
