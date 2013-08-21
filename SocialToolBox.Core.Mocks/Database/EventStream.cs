using SocialToolBox.Core.Database;

namespace SocialToolBox.Core.Mocks.Database
{
    public class EventStream : IEventStream
    {
        /// <summary>
        /// The actual inner database driver, with its known type.
        /// </summary>
        public readonly DatabaseDriver InnerDriver;

        /// <summary>
        /// The name of this stream.
        /// </summary>
        private readonly string _name;

        public EventStream(string name, DatabaseDriver driver)
        {
            InnerDriver = driver;
            _name = name;
        }

        public IDatabaseDriver Driver
        {
            get { return InnerDriver; }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
