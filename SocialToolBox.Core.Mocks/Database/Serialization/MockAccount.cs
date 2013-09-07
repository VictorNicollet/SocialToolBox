using System;

namespace SocialToolBox.Core.Mocks.Database.Serialization
{
    [Serializable]
    public class MockAccount : IEquatable<MockAccount>
    {
        [Serializable]
        public class HashedPassword : IEquatable<HashedPassword>
        {
            public int BcryptIterationCount;
            public string Hash;

            public bool Equals(HashedPassword other)
            {
                return other.BcryptIterationCount == BcryptIterationCount
                       && other.Hash == Hash;
            }
        }

        public string Name;
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
