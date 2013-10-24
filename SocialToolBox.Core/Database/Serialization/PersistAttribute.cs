using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SocialToolBox.Core.Database.Serialization
{
    /// <summary>
    /// Marks a persistent class, allows serialization using <see cref="UntypedSerializer"/>.
    /// Classes not marked with this attribute (and which are not a standard type)
    /// will not be serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PersistAttribute : Attribute
    {
        /// <summary>
        /// The name should be a unique, long-term, human-readable identifier for
        /// the type. 
        /// </summary>
        public readonly string Name;

        public PersistAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Get the name bound to a persistent type.
        /// </summary>
        public static string GetName(Type type)
        {
            return type.GetCustomAttributes(typeof (PersistAttribute), true).OfType<PersistAttribute>()
                .Select(asPersist => asPersist.Name).FirstOrDefault();
        }

        /// <summary>
        /// A dictionary of all persistent types, indexed by their name.
        /// Filled with attributes extracted from registered assemblies.
        /// </summary>
        private static readonly Dictionary<string, Type> TypeByName = 
            new Dictionary<string, Type>();

        /// <summary>
        /// All the assemblies registered so far.
        /// </summary>
        private static readonly HashSet<string> RegisteredAssemblies =
            new HashSet<string>();

        /// <summary>
        /// Whether or not the code has registered all currently loaded
        /// assemblies.
        /// </summary>
        private static bool _hasLoadedApplicationAssemblies;

        /// <summary>
        /// Registers all currently loaded assemblies.
        /// </summary>
        private static void RegisterAllLoaded()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                Register(assembly);

            _hasLoadedApplicationAssemblies = true;
        }

        /// <summary>
        /// Registers an assembly with the persistence system. This causes
        /// all types in that assembly to become available by name.
        /// </summary>
        /// <remarks>
        /// It is only necessary to call this function if the assembly is
        /// loaded after the first call to <see cref="GetTypeByName"/> or
        /// <see cref="GetAllTypeNames"/>.
        /// </remarks>
        public static void Register(Assembly assembly)
        {
            lock (RegisteredAssemblies)
            {
                if (RegisteredAssemblies.Contains(assembly.FullName)) return;

                foreach (var type in assembly.DefinedTypes)
                {
                    var name = GetName(type);
                    if (null != name)
                    {
                        if (TypeByName.ContainsKey(name))
                            throw new Exception(
                                string.Format("Types '{0}' and '{1}' both persist under name '{2}'.",
                                    type, TypeByName[name], name));    

                        TypeByName.Add(name, type);
                    }
                }

                RegisteredAssemblies.Add(assembly.FullName);
            }
        }

        /// <summary>
        /// Get the persistent type with the specified name.
        /// </summary>
        /// <remarks>
        /// Will only search inside the registered assemblies.
        /// </remarks>
        public static Type GetTypeByName(string name)
        {
            if (!_hasLoadedApplicationAssemblies) RegisterAllLoaded();

            Type found;
            TypeByName.TryGetValue(name, out found);
            return found;
        }

        /// <summary>
        /// Returns all registered persistent type names.
        /// </summary>
        public static IEnumerable<string> GetAllTypeNames()
        {
            if (!_hasLoadedApplicationAssemblies) RegisterAllLoaded();
            return TypeByName.Keys;
        }
    }
}
