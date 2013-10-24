using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SocialToolBox.Core.Present;

namespace SocialToolBox.Core.Tests.Present
{
    [TestFixture]
    public class html_output
    {
        public HtmlOutput Output;

        [SetUp]
        public void SetUp()
        {
            Output = new HtmlOutput();
        }

        private void Is(string expected)
        {
            Assert.AreEqual(expected, Output.Build().Result.ToString());
        }

        public void Nothing(Task<bool> b) {}

        public IPair<Task,Action> Waiter()
        {
            var tcs = new TaskCompletionSource<bool>();
            return Pair.Make<Task, Action>(tcs.Task.ContinueWith(Nothing), () => tcs.SetResult(true));
        }

        [Test]
        public void initially_empty()
        {
            Is("");
        }

        [Test]
        public void verbatim_is_kept()
        {
            Output.AddVerbatim("<&>");
            Is("<&>");
        }

        [Test]
        public void normal_is_escaped()
        {
            Output.Add("<&>");
            Is("&lt;&amp;&gt;");
        }

        [Test]
        public void concatenation()
        {
            Output.Add("<&>");
            Output.AddVerbatim("<&>");
            Is("&lt;&amp;&gt;<&>");
        }

        [Test]
        public void with_task()
        {
            Output.Insert(async o => o.Add("!"));
            Is("!");
        }

        [Test]
        public void with_pending_task()
        {
            var a = Waiter();
            Output.Insert(async o => { await a.First; o.Add("A"); });
            Output.Add("B");

            a.Second();        
            Is("AB");
        }

        [Test]
        public void with_pending_tasks()
        {
            var a = Waiter();
            var b = Waiter();
            Output.Insert(async o => { await a.First; o.Add("A"); });
            Output.Insert(async o => { await b.First; o.Add("B"); });

            b.Second();
            a.Second();
            Is("AB");
        }
    }
}
