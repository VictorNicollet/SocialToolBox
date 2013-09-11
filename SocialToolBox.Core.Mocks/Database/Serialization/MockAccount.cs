using System;
using SocialToolBox.Core.Database.Serialization;

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
                   && Password.Equals(other.Password);
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
    }
}
