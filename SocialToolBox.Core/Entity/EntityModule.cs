using System;
using System.Collections.Generic;
using System.Linq;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Index;
using SocialToolBox.Core.Database.Projection;
using SocialToolBox.Core.Entity.Event;
using SocialToolBox.Core.Entity.Projection;

namespace SocialToolBox.Core.Entity
{
    /// <summary>
    /// A representation of the "entity" module. Lets other modules declare arbitrary
    /// things as entities.
    /// </summary>
    public class EntityModule
    {
        /// <summary>
        /// The database driver on which this module runs.
        /// </summary>
        public readonly IDatabaseDriver Driver;

        /// <summary>
        /// The entity pages, by identifier. Reading this property will
        /// compile the module.
        /// </summary>
        public IStore<IEntityPage> Pages
        {
            get
            {
                if (!Compiled) Compile();
                return _pages;
            }
        }

        /// <summary>
        /// The entity pages, by identifier. Is null until the projection is created
        /// and compiled.
        /// </summary>
        private IStore<IEntityPage> _pages;

        /// <summary>
        /// An index of pages, sorted by their name.
        /// </summary>
        private IIndex<NoKey, StringKey> _pageByTitle;

        /// <summary>
        /// An index of pages, sorted by their name. Reading this property
        /// will compile the module.
        /// </summary>
        public IIndex<NoKey, StringKey> PageByTitle
        {
            get
            {
                if (!Compiled) Compile();
                return _pageByTitle;
            }
        }

        /// <summary>
        /// Have the module projections aready been compiled ? 
        /// </summary>
        public bool Compiled { get; private set; }

        /// <summary>
        /// Compiles the projections using any streams registered so far.
        /// </summary>
        public void Compile()
        {
            if (Compiled) return;

            var pagesProjection =
                Driver.CreateProjection<IEntityPageEvent>("SocialToolBox.Core.Entity.EntityModule.Pages");

            _pages = pagesProjection.CreateStore(
                "Pages", ev => ev.EntityId, PageEventVisitor, EventStreams.ToArray());

            _pageByTitle = pagesProjection.CreateIndex(
                "PagesByTitle", _pages, GetPageTitle);

            pagesProjection.Compile();
            
            Compiled = true;
        }

        /// <summary>
        /// All streams from which events will be read in order to construct
        /// entity views.
        /// </summary>
        public IEnumerable<IEventStream> EventStreams { get { return _eventStreams; } }

        /// <summary>
        /// The list of event streams, in order.
        /// </summary>
        private readonly List<IEventStream> _eventStreams = new List<IEventStream>(); 

        /// <summary>
        /// Add an event stream to the list of streams read in order to 
        /// construct entity views. See the definitions of individual 
        /// visitors to see which interfaces must be implemented by the
        /// events. 
        /// </summary>
        /// <remarks>
        /// Adding event streams is only possible until the module is 
        /// compiled.
        /// </remarks>
        public void AddEventStream(IEventStream stream)
        {
            if (Compiled)
                throw new InvalidOperationException("Entity Module is already compiled.");

            if (_eventStreams.Contains(stream)) return;
            _eventStreams.Add(stream);
        }

        /// <summary>
        /// The visitor used to update entity pages. 
        /// </summary>
        public readonly Visitor<IEntityPage, IEntityPage> PageEventVisitor =
            new Visitor<IEntityPage, IEntityPage>();

        public EntityModule(IDatabaseDriver driver)
        {
            Driver = driver;
        }

        /// <summary>
        /// Extracts the title of a page, or null if no page is provided.
        /// </summary>
        private static IPair<NoKey, StringKey> GetPageTitle(IEntityPage page)
        {
            if (page == null) return null;
            return Pair.Make(new NoKey(), new StringKey(page.Title));
        }
    }
}
