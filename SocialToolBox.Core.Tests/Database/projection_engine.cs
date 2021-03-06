﻿using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.EventStream;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Mocks.Database;

namespace SocialToolBox.Core.Tests.Database
{
    [TestFixture]
    public class projection_engine
    {
        public ProjectionEngine Engine;
        public IEventStream A;
        public IEventStream B;
        public IClockRegistry Clocks;
        public ICursor Cursor;

        [Persist("SocialToolBox.Core.Tests.Database.projection_engine.Event")]
        public class Event
        {
            [PersistMember(0)]
            public string Value;

            public Event() { }

            public Event(string value)
            {
                Value = value;
            }

            public override string ToString()
            {
                return Value;
            }
        }

        [SetUp]
        public void SetUp()
        {
            var driver = new DatabaseDriver();
            Engine = driver.Projections;
            A = driver.GetEventStream("A", true);
            B = driver.GetEventStream("B", true);
            Clocks = driver.ClockRegistry;
            Cursor = driver.OpenReadWriteCursor();
        }

        [Test]
        public void empty_does_nothing()
        {
            Engine.Run();
        }

        private class Projector : IProjector<Event>
        {
            public readonly StringBuilder Contents;
            public string Name { get; set; }            
            public IEventStream[] Streams { get; set; }

            public Projector(params IEventStream[] streams)
            {
                Contents = new StringBuilder();
                Streams = streams;
            }

            // ReSharper disable CSharpWarnings::CS1998
            public async Task ProcessEvent(EventInStream<Event> ev, IProjectCursor t)
            // ReSharper restore CSharpWarnings::CS1998
            {
                Contents.Append(ev.Event);
                Contents.Append('\n');

                ProjectCursor.RegisterCommit(t, Name, Commit);
            }

            private void Commit()
            {
                Contents.Append("[COMMIT]\n");
            }
        }

        [Test]
        public void simple_projector()
        {
            var proj = new Projector(A) { Name = "TEST" };

            A.AddEvent(new Event("A1"), Cursor);
            A.AddEvent(new Event("A2"), Cursor);
            Engine.Register(proj);

            Assert.AreEqual("", proj.Contents.ToString());

            Engine.Run();

            Assert.AreEqual("A1\nA2\n[COMMIT]\n", proj.Contents.ToString());
        }

        [Test]
        public void simple_projection_vector_clock()
        {
            var proj = new Projector(A) { Name = "TEST" };

            Clocks.SaveProjection("TEST", VectorClock.Unserialize("A:1")).Wait();

            A.AddEvent(new Event("A1"), Cursor);
            A.AddEvent(new Event("A2"), Cursor);
            Engine.Register(proj);
            Engine.Run();

            Assert.AreEqual("A2\n[COMMIT]\n", proj.Contents.ToString());
            Assert.AreEqual(VectorClock.Unserialize("A:2"), Clocks.LoadProjection("TEST").Result);
        }

        [Test]
        public void simple_projection_autocommit()
        {
            Engine.MaxLoad = 5;

            var proj = new Projector(A) { Name = "TEST" };

            A.AddEvent(new Event("A1"), Cursor);
            A.AddEvent(new Event("A2"), Cursor);
            A.AddEvent(new Event("A3"), Cursor);
            A.AddEvent(new Event("A4"), Cursor);
            A.AddEvent(new Event("A5"), Cursor);
            A.AddEvent(new Event("A6"), Cursor);
            A.AddEvent(new Event("A7"), Cursor);
            Engine.Register(proj);
            Engine.Run();

            Assert.AreEqual("A1\nA2\nA3\nA4\nA5\n[COMMIT]\nA6\nA7\n[COMMIT]\n", 
                proj.Contents.ToString());            
        }

        [Test]
        public void double_projection()
        {
            var proj1 = new Projector(A) { Name = "TEST1" };
            var proj2 = new Projector(A) { Name = "TEST2" };

            A.AddEvent(new Event("A1"), Cursor);
            A.AddEvent(new Event("A2"), Cursor);
            Engine.Register(proj1);
            Engine.Register(proj2);
            Engine.Run();

            Assert.AreEqual("A1\nA2\n[COMMIT]\n", proj1.Contents.ToString());
            Assert.AreEqual("A1\nA2\n[COMMIT]\n", proj2.Contents.ToString());
        }

        [Test]
        public void double_projection_double_stream()
        {
            var proj1 = new Projector(A) { Name = "TEST1" };
            var proj2 = new Projector(A,B) { Name = "TEST2" };

            A.AddEvent(new Event("A1"), Cursor);
            A.AddEvent(new Event("A2"), Cursor);
            B.AddEvent(new Event("B1"), Cursor);
            B.AddEvent(new Event("B2"), Cursor);
            Engine.Register(proj1);
            Engine.Register(proj2);
            Engine.Run();

            Assert.AreEqual("A1\nA2\n[COMMIT]\n", proj1.Contents.ToString());
            Assert.AreEqual("A1\nA2\nB1\nB2\n[COMMIT]\n", proj2.Contents.ToString());
        }
    }
}
