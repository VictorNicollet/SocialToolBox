using System;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Mocks.Database.Events;

namespace SocialToolBox.Core.Mocks.Database.Serialization
{
    [Persist("SocialToolBox.Core.Mocks.MockAccount")]
    public class MockAccount : IEquatable<MockAccount>
    {
        [Persist("SocialToolBox.Core.Mocks.MockAccount.HashedPassword")]
        public class HashedPassword : IEquatable<HashedPassword>
        {
            [PersistMember(0)]
            public int BcryptIterationCount;
            
            [PersistMember(1)]
            public string Hash;

            public bool Equals(HashedPassword other)
            {
                if (other == null) return false;

                return other.BcryptIterationCount == BcryptIterationCount
                       && other.Hash == Hash;
            }
        }

        [PersistMember(0)]
        public string Name;

        [PersistMember(1)]
        public HashedPassword Password;

        public bool Equals(MockAccount other)
        {
            return other.Name == Name
                && (Password == null ? other.Password == null : Password.Equals(other.Password));
        }

        public static MockAccount Bob = new MockAccount
        {
            Name = "Bob",
            Password = new HashedPassword
            {
                BcryptIterationCount = 10,
                Hash = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
            }
        };

        public static readonly Visitor<MockAccount, MockAccount> ApplyEvent =
            new Visitor<MockAccount, MockAccount>();

        static MockAccount()
        {
            ApplyEvent.On<MockAccountCreated>((e,i) => new MockAccount{Name = e.Name});
            ApplyEvent.On<MockAccountDeleted>((e,i) => null);
            
            ApplyEvent.On<MockAccountNameUpdated>((e, i) =>
            {
                if (i == null) return null;
                i.Name = e.Name;
                return i;
            });

            ApplyEvent.On<MockAccountPasswordUpdated>((e, i) =>
            {
                if (i == null) return null;
                i.Password = e.Password;
                return i;
            });
        }
    }
}
