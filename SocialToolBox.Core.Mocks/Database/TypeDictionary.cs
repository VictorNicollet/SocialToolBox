using System.Collections.Generic;
using SocialToolBox.Core.Database;

namespace SocialToolBox.Core.Mocks.Database
{
    public class TypeDictionary : ITypeDictionary
    {
        public readonly Dictionary<uint, string> TypeById;
        public readonly Dictionary<string, uint> IdByType;
        public uint Next;

        public TypeDictionary()
        {
            Next = 0;
            TypeById = new Dictionary<uint, string>();
            IdByType = new Dictionary<string, uint>();
        }

        public uint FindIdentifier(string persistentName)
        {
            // Assigning identifiers ensures the type is present
            // in the dictionary
            AssignIdentifiers(new[] { persistentName });
            return IdByType[persistentName];
        }

        public string FindType(uint id)
        {
            string t;
            TypeById.TryGetValue(id, out t);
            return t;
        }

        public void AssignIdentifiers(string[] persistentNames)
        {
            foreach (var t in persistentNames)
            {
                if (IdByType.ContainsKey(t)) continue;
                IdByType.Add(t, Next);
                TypeById.Add(Next, t);
                Next++;
            }
        }
    }
}
