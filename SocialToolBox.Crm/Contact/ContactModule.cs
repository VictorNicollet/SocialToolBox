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
    }
}
