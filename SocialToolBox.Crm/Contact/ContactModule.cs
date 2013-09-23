using SocialToolBox.Core.Database;

namespace SocialToolBox.Crm.Contact
{
    public class ContactModule
    {
        /// <summary>
        /// The name of the <see cref="Stream"/>.
        /// </summary>
        public const string StreamName = "SocialToolBox.Crm.Contact.ContactModule.Stream";

        /// <summary>
        /// The database driver used by this module.
        /// </summary>
        public readonly IDatabaseDriver Driver;

        /// <summary>
        /// The stream to which all contact-related events should be appended to.
        /// </summary>
        public readonly IEventStream Stream;

        public ContactModule(IDatabaseDriver driver)
        {
            Driver = driver;
            Stream = driver.GetEventStream(StreamName, true);
        }

        /// <summary>
        /// Has this module been compiled yet ? 
        /// </summary>
        public bool Compiled { get; private set; }

        /// <summary>
        /// Create and compile all projections in this module.
        /// </summary>
        public void Compile()
        {
            Compiled = true;
        }
    }
}
