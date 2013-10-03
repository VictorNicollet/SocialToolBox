using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialToolBox.Core.Database.EventStream;

namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// A multiple projector, dispatches events to several projectors in
    /// order of definition.
    /// </summary>
    public class MultiProjector<T> : IProjector<T> where T : class
    {        
        /// <summary>
        /// The list of all registered projectors.
        /// </summary>
        private readonly List<IProjector<T>> _projectors =
            new List<IProjector<T>>();

        /// <summary>
        /// The list of the names of all registered manual operations.
        /// </summary>
        private readonly List<string> _manualOperations = 
            new List<string>();

        /// <summary>
        /// Register a new projector.
        /// </summary>
        public void Register(IProjector<T> projector)
        {
            _projectors.Add(projector);
            _streams = null;
        }

        /// <summary>
        /// Create a multi-projector with the specified name.
        /// </summary>
        public MultiProjector(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The concatenation of all projector names, separated by semicolons.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The cached value returned by <see cref="Streams"/>. Null if
        /// cache is expired.
        /// </summary>
        private IEventStream[] _streams;

        /// <summary>
        /// All event streams used by at least one projector, in the correct order
        /// (assuming no ordering conflicts occurred).
        /// </summary>
        public IEventStream[] Streams
        {
            get
            {
                if (_streams != null) return _streams;
                    
                var streams = new List<IEventStream>();

                // We will be manipulating the streams by key, so create a 
                // dictionary of all streams.
                var byKey = new Dictionary<string, IEventStream>();

                foreach (var p in _projectors)
                    foreach (var s in p.Streams)
                        if (!byKey.ContainsKey(s.Name))
                            byKey.Add(s.Name, s);

                // Special case if there is only one stream involved:
                // handy optimization.
                if (byKey.Count == 1)
                    return new[] {byKey.First().Value};

                // PREPARE A TOPOLOGICAL SORT

                var streamsAfter = new HashSet<KeyValuePair<string,string>>();
                foreach (var p in _projectors)
                {
                    for (var i = 0; i < p.Streams.Length; ++i)
                    {
                        var name = p.Streams[i].Name;

                        for (var j = i + 1; j < p.Streams.Length; ++j)
                            if (name != p.Streams[j].Name)
                                streamsAfter.Add(new KeyValuePair<string, string>(name, p.Streams[j].Name));
                    }
                }

                var unsorted = byKey.Keys.ToList();

                while (unsorted.Count > 0)
                {
                    var thoseWithPrevious = 
                        new HashSet<string>(streamsAfter.Select(kv => kv.Value));

                    var withoutPrevious = 
                        unsorted.FindIndex(s => !thoseWithPrevious.Contains(s));

                    if (withoutPrevious == -1)
                    {
                        throw new InvalidOperationException(
                            string.Format("Cyclical event stream dependency in '{0}' for streams:\n  {1}",
                                Name,
                                string.Join("\n  ", unsorted)));
                    }

                    var name = unsorted[withoutPrevious];
                    unsorted.RemoveAt(withoutPrevious);
                    streamsAfter.RemoveWhere(kv => kv.Key == name);
                    streams.Add(byKey[name]);
                }

                _streams = streams.ToArray();
                return _streams;
            }
        }

        /// <summary>
        /// Pass the event to all projectors, in order, as long as they have been 
        /// registered to receive events from that stream.
        /// </summary>
        public async Task ProcessEvent(EventInStream<T> ev, IProjectCursor t)
        {
            foreach (var p in _projectors) 
                if (p.Streams.Any(s => s == ev.Stream))
                    await p.ProcessEvent(ev, t);
        }

        /// <summary>
        /// Register a new manual operation. 
        /// </summary>
        public void RegisterManual(string name)
        {
            _manualOperations.Add(name);
        }
    }
}
