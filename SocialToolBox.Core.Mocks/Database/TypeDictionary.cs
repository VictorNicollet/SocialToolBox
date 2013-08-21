using System;
using System.Collections.Generic;
using SocialToolBox.Core.Database;

namespace SocialToolBox.Core.Mocks.Database
{
    public class TypeDictionary : ITypeDictionary
    {
        public readonly Dictionary<int, Type> TypeById;
        public readonly Dictionary<Type, int> IdByType;
        public int Next;

        public TypeDictionary()
        {
            Next = 0;
            TypeById = new Dictionary<int, Type>();
            IdByType = new Dictionary<Type, int>();
        }

        public int FindIdentifier(Type t)
        {
            // Assigning identifiers ensures the type is present
            // in the dictionary
            AssignIdentifiers(new []{ t });
            return IdByType[t];
        }

        public Type FindType(int id)
        {
            Type t;
            TypeById.TryGetValue(id, out t);
            return t;
        }

        public void AssignIdentifiers(Type[] types)
        {
            foreach (var t in types)
            {
                if (IdByType.ContainsKey(t)) continue;
                IdByType.Add(t, Next);
                TypeById.Add(Next, t);
                Next++;
            }
        }
    }
}
