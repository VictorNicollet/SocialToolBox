using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Crm.Contact.Event;

namespace SocialToolBox.Sample.Web
{
    /// <summary>
    /// Data initially available in the system.
    /// </summary>
    public static class InitialData
    {
        public static readonly Id UserVictorNicollet = Id.Parse("aaaaaaaaaaa");
        public static readonly Id ContactBenjaminFranklin = Id.Parse("aaaaaaaaaab");
        public static readonly Id ContactJuliusCaesar = Id.Parse("aaaaaaaaaac");

        /// <summary>
        /// Generates sample data.
        /// </summary>
        public static void AddTo(SocialModules modules)
        {
            var t = modules.Database.OpenReadWriteCursor();
            AddContactsTo(modules, t);
        }

        /// <summary>
        /// Generates sample contacts.
        /// </summary>
        private static void AddContactsTo(SocialModules modules, ICursor t)
        {
            foreach (var ev in new IContactEvent[]
            {
                new ContactCreated(ContactBenjaminFranklin, DateTime.Parse("2013/09/26"),UserVictorNicollet),
                new ContactNameUpdated(ContactBenjaminFranklin, DateTime.Parse("2013/09/26"),UserVictorNicollet,"Benjamin","Franklin"),                
                new ContactCreated(ContactJuliusCaesar, DateTime.Parse("2013/09/27"),UserVictorNicollet),
                new ContactNameUpdated(ContactJuliusCaesar, DateTime.Parse("2013/09/27"),UserVictorNicollet,"Julius","Caesar")                                
            }) modules.Contacts.Stream.AddEvent(ev, t);
        }
    }
}