using System;
using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Entity;
using SocialToolBox.Core.Mocks.Database;
using SocialToolBox.Crm.Contact;
using SocialToolBox.Crm.Contact.Event;
using SocialToolBox.Crm.Contact.Projection;

namespace SocialToolBox.Crm.Tests.Contact.Projection
{
    [TestFixture]
    public class contact_as_entity_page
    {
        public ContactModule Contacts;
        public EntityModule Entities;
        public ProjectionEngine Projections;
        public ITransaction Transaction;

        public Id IdA = Id.Parse("aaaaaaaaaaa");
        public Id IdUser = Id.Parse("user123user");

        [SetUp]
        public void SetUp()
        {
            var driver = new DatabaseDriver();
            Contacts = new ContactModule(driver);
            Entities = new EntityModule(driver);
            Contacts.RegisterContactsAsEntities(Entities);
            Projections = driver.Projections;
            Transaction = driver.StartReadWriteTransaction();
            
            Entities.Compile();
        }
        [Test]
        public void initially_empty()
        {
            Assert.IsNull(Entities.Pages.Get(IdA).Result);
        }

        [Test]
        public void create_provides_title()
        {
            Contacts.Stream.AddEvent(new ContactCreated(IdA, DateTime.Parse("2011/05/14"), IdUser), Transaction);
            Projections.Run();

            Assert.AreEqual(typeof(ContactAsEntityPage), Entities.Pages.Get(IdA).Result.GetType());
            Assert.IsNull(Entities.Pages.Get(IdA).Result.Title);
        }

        [Test]
        public void update_changes_title()
        {
            Contacts.Stream.AddEvent(new ContactCreated(IdA, DateTime.Parse("2011/05/14"), IdUser), Transaction);
            Contacts.Stream.AddEvent(new ContactNameUpdated(IdA, DateTime.Parse("2011/05/14"), IdUser, "Victor", "Nicollet"), Transaction);
            Projections.Run();

            Assert.AreEqual("Victor Nicollet", Entities.Pages.Get(IdA).Result.Title);
        }

        [Test]
        public void delete_removes_page()
        {
            Contacts.Stream.AddEvent(new ContactCreated(IdA, DateTime.Parse("2011/05/14"), IdUser), Transaction);
            Contacts.Stream.AddEvent(new ContactDeleted(IdA, DateTime.Parse("2011/05/14"), IdUser), Transaction);
            Projections.Run();

            Assert.IsNull(Entities.Pages.Get(IdA).Result);
        }

        [Test]
        public void missing_without_create()
        {
            Contacts.Stream.AddEvent(new ContactNameUpdated(IdA, DateTime.Parse("2011/05/14"), IdUser, "Victor", "Nicollet"), Transaction);
            Projections.Run();

            Assert.IsNull(Entities.Pages.Get(IdA).Result);
        }
    }
}
