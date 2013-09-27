using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Entity;
using SocialToolBox.Core.Mocks.Database.Events;
using SocialToolBox.Core.Mocks.Database.Serialization;

namespace SocialToolBox.Core.Mocks.Entity
{
    /// <summary>
    /// Fills an <see cref="EntityModule"/> with mock data.
    /// </summary>
    public static class EntityModuleMock
    {
        /// <summary>
        /// Fills an <see cref="EntityModule"/> with mock data. Call this before
        /// the module is compiled.
        /// </summary>
        public static void Fill(EntityModule module)
        {
            var driver = module.Driver;

            var stream = driver.GetEventStream("mock", true);
            module.AddEventStream(stream);
        
            MockAccountAsEntityPage.ExtendVisitor(module.PageEventVisitor);

            var t = driver.OpenReadWriteCursor();

            stream.AddEvent(new MockAccountCreated(IdAlice, NameAlice, Creation), t);
            stream.AddEvent(new MockAccountCreated(IdBob, "", Creation), t);
            stream.AddEvent(new MockAccountNameUpdated(IdBob, Creation, NameBob), t);
        }

        public static readonly Id IdAuthor = Id.Parse("author00000");

        public static readonly Id IdAlice  = Id.Parse("000000alice");
        public const string NameAlice = "Alice";

        public static readonly Id IdBob    = Id.Parse("00000000bob");
        public const string NameBob = "Bob";

        public static readonly DateTime Creation = DateTime.Parse("2013/09/26");
    }
}
