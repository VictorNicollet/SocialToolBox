using System;
using System.Threading;

namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// Manages a background thread that calls <see cref="ProjectionEngine.Run"/>
    /// in a loop forever.
    /// </summary>
    public class ProjectionEngineThread
    {
        /// <summary>
        /// The thread which does the actual work.
        /// </summary>
        private Thread _worker;

        /// <summary>
        /// The engine on which the run function is called.
        /// </summary>
        private readonly ProjectionEngine _engine;

        /// <summary>
        /// The last time the worker thread did any work. Access to this value
        /// is not secured with locks, because there are no risks if concurrent
        /// access does happen.
        /// </summary>
        private DateTime _ping;

        /// <summary>
        /// After this duration has elapsed without a ping, consider the worker
        /// thread dead.
        /// </summary>
        public static readonly TimeSpan DeathDelay = TimeSpan.FromSeconds(30);

        public ProjectionEngineThread(ProjectionEngine engine)
        {
            _engine = engine;
        }

        /// <summary>
        /// Starts the thread, if it is not already running.
        /// </summary>
        public void Start()
        {
            lock (this)
            {
                // The thread is alive. Do nothing.
                if (_worker != null || _ping > DateTime.Now + DeathDelay) return;
                
                // The thread has timed out, kill it.
                if (_worker != null) _worker.Abort();
            
                _worker = new Thread(ThreadMain)
                {
                    Name="SocialToolBox Background",
                    IsBackground = true                
                };

                _worker.Start();
            }
        }

        /// <summary>
        /// What the background thread actually does.
        /// </summary>
        private void ThreadMain()
        {
            while (true)
            {
                _ping = DateTime.Now;
                _engine.Run();
            }
        }
    }
}
