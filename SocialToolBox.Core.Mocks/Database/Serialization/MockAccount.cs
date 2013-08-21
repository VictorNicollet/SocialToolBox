using System;

namespace SocialToolBox.Core.Mocks.Database.Serialization
{
    [Serializable]
    public class MockAccount
    {
        [Serializable]
        public class HashedPassword
        {
            public int BcryptIterationCount;
            public string Hash;
        }

        public string Name;
        public HashedPassword Password;
    }
}
