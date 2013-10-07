using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Entity.Event;
using SocialToolBox.Core.Entity.Projection;
using SocialToolBox.Core.Mocks.Database.Events;

namespace SocialToolBox.Core.Mocks.Database.Serialization
{
    /// <summary>
    /// A mock account as an entity page.
    /// </summary>
    [Persist("SocialToolBox.Core.Mocks.Database.Serialization.MockAccountAsEntityPage")]
    public class MockAccountAsEntityPage : IEntityPage
    {
        /// <summary>
        /// The name of the mock account.
        /// </summary>
        [PersistMember(0)]
        public string Name { get; private set; }

        /// <summary>
        /// The title of the entity page.
        /// </summary>
        public string Title { get { return Name; } }

        /// <summary>
        /// Extends an entity page visitor.
        /// </summary>
        public static void ExtendVisitor(Visitor<IEntityPageEvent, IEntityPage, IEntityPage> visitor)
        {
            visitor.On<MockAccountCreated>((e,i) => new MockAccountAsEntityPage{Name = e.Name});
            visitor.On<MockAccountDeleted>((e,i) => null);
            visitor.On<MockAccountNameUpdated>((e, i) =>
            {
                var old = i != null ? i as MockAccountAsEntityPage : null;
                if (old != null) old.Name = e.Name;
                return old ?? i;
            });
        }
    }
}
