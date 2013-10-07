using System;
using NUnit.Framework;
using SocialToolBox.Cms.Page;
using SocialToolBox.Cms.Page.Event;
using SocialToolBox.Cms.Page.Projection;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Entity;
using SocialToolBox.Core.Mocks.Database;

namespace SocialToolBox.Cms.Tests.Page.Projection
{
    [TestFixture]
    public class Page_as_entity_page
    {
        public PageModule Pages;
        public EntityModule Entities;
        public ProjectionEngine Projections;
        public ICursor Cursor;

        public Id IdA = Id.Parse("aaaaaaaaaaa");
        public Id IdUser = Id.Parse("user123user");

        [SetUp]
        public void SetUp()
        {
            var driver = new DatabaseDriver();
            Pages = new PageModule(driver);
            Entities = new EntityModule(driver);
            Pages.RegisterPagesAsEntities(Entities);
            Projections = driver.Projections;
            Cursor = driver.OpenReadWriteCursor();
            
            Entities.Compile();
        }
        [Test]
        public void initially_empty()
        {
            Assert.IsNull(Entities.Pages.Get(IdA, Cursor).Result);
        }

        [Test]
        public void create_provides_title()
        {
            Pages.Stream.AddEvent(new PageCreated(IdA, DateTime.Parse("2011/05/14"), IdUser), Cursor);
            Projections.Run();

            Assert.AreEqual(typeof(PageAsEntityPage), Entities.Pages.Get(IdA, Cursor).Result.GetType());
            Assert.IsNull(Entities.Pages.Get(IdA, Cursor).Result.Title);
        }

        [Test]
        public void update_changes_title()
        {
            Pages.Stream.AddEvent(new PageCreated(IdA, DateTime.Parse("2011/05/14"), IdUser), Cursor);
            Pages.Stream.AddEvent(new PageTitleUpdated(IdA, DateTime.Parse("2011/05/14"), IdUser, "About us"), Cursor);
            Projections.Run();

            var page = Entities.Pages.Get(IdA, Cursor).Result;

            Assert.AreEqual("About us", page.Title);
        }

        [Test]
        public void delete_removes_page()
        {
            Pages.Stream.AddEvent(new PageCreated(IdA, DateTime.Parse("2011/05/14"), IdUser), Cursor);
            Pages.Stream.AddEvent(new PageDeleted(IdA, DateTime.Parse("2011/05/14"), IdUser), Cursor);
            Projections.Run();

            Assert.IsNull(Entities.Pages.Get(IdA, Cursor).Result);
        }

        [Test]
        public void missing_without_create()
        {
            Pages.Stream.AddEvent(new PageTitleUpdated(IdA, DateTime.Parse("2011/05/14"), IdUser, "About us"), Cursor);
            Projections.Run();

            Assert.IsNull(Entities.Pages.Get(IdA, Cursor).Result);
        }
    }
}
