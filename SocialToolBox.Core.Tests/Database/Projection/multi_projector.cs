using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.EventStream;
using SocialToolBox.Core.Database.Projection;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Mocks.Database;

namespace SocialToolBox.Core.Tests.Database.Projection
{
    [TestFixture]
    public class multi_projector
    {
        public IEventStream A;
        public IEventStream B;
        public IEventStream C;

        public Projector Ab;
        public Projector Bc;
        public Projector Ac;
        public Projector Ca;

        public MultiProjector<Event> Multi;

        public IProjectTransaction PTransaction;
        public ITransaction Transaction;

        [Persist("SocialToolBox.Core.Tests.Database.Projection.multi_projector.Event")]
        public class Event
        {
            [PersistMember(0)] public string Value;

            public Event() {}        

            public Event(string value)
            {
                Value = value;
            }

            public override string ToString()
            {
                return Value;
            }
        }

        public class Projector : IProjector<Event>
        {
            public Projector(params IEventStream[] streams)
            {
                Streams = streams;
            }

            public readonly List<string> Events = new List<string>();

            public string Name { get { return string.Concat(Streams.Select(s => s.Name)); } }

            // ReSharper disable CSharpWarnings::CS1998
            public async Task ProcessEvent(EventInStream<Event> ev, IProjectTransaction t)
            // ReSharper restore CSharpWarnings::CS1998
            {
                Events.Add(ev.Event.ToString());
            }

            public IEventStream[] Streams { get; private set; }
        }

        [SetUp]
        public void SetUp()
        {
            var driver = new DatabaseDriver();
            A = driver.GetEventStream("A", true);
            B = driver.GetEventStream("B", true);
            C = driver.GetEventStream("C", true);

            Ab = new Projector(A,B);
            Bc = new Projector(B,C);
            Ac = new Projector(A,C);
            Ca = new Projector(C,A);

            Multi = new MultiProjector<Event>("Test");

            Transaction = driver.StartReadWriteTransaction();
            PTransaction = driver.StartProjectorTransaction();
        }

        public void With(params Projector[] projectors)
        {
            foreach (var p in projectors) Multi.Register(p);
        }

        public IMultiStreamIterator<Event> Iterator
        {
            get { return FromEventStream.EachOfType<Event>(new VectorClock(), PTransaction, Multi.Streams); }
        }
            
        [Test]
        public void single_projector_streams()
        {
            With(Ab);
            CollectionAssert.AreEqual(new[]{A, B}, Multi.Streams);
        }

        [Test]
        public void multi_projector_streams()
        {
            With(Ab,Bc);
            CollectionAssert.AreEqual(new[]{A,B,C}, Multi.Streams);
        }

        [Test]
        public void cycle_projector_streams()
        {
            // The cycle is detected
            With(Ab,Bc,Ca);
            Assert.Throws<InvalidOperationException>(() => 
                Assert.IsNotNull(Multi.Streams));
        }

        [Test]
        public void everything_receives_events()
        {
            B.AddEvent(new Event("B1"), Transaction);
            With(Ab,Bc);

            var iter = Iterator;
            Multi.ProcessEvent(iter.Next(), PTransaction).Wait();

            CollectionAssert.AreEqual(new[] { "B1" }, Ab.Events);
            CollectionAssert.AreEqual(new[] { "B1" }, Bc.Events);
        }

        [Test]
        public void events_filtered()
        {
            A.AddEvent(new Event("A1"), Transaction);
            B.AddEvent(new Event("B1"), Transaction);
            C.AddEvent(new Event("C1"), Transaction);
            With(Ab,Bc);

            foreach (var e in Iterator)
                Multi.ProcessEvent(e, PTransaction).Wait();

            CollectionAssert.AreEqual(new[] { "A1", "B1" }, Ab.Events);
            CollectionAssert.AreEqual(new[] { "B1", "C1" }, Bc.Events);
        }
    }
}
