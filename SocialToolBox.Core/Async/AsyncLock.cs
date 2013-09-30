using System;
using System.Threading.Tasks;

namespace SocialToolBox.Core.Async
{
    /// <summary>
    /// An asynchronous mutex lock
    /// </summary>
    /// <remarks>
    /// Inspired by http://blogs.msdn.com/b/pfxteam/archive/2012/02/12/10266988.aspx
    /// </remarks>
    public class AsyncLock
    {
        private readonly AsyncSemaphore _semaphore = new AsyncSemaphore();

        /// <summary>
        /// Returned by the lock method, releases the lock when disposed.
        /// </summary>
        public struct Releaser : IDisposable
        {
            private readonly AsyncLock _toRelease;

            public Releaser(AsyncLock toRelease)
            {
                _toRelease = toRelease;
            }

            public void Dispose()
            {
                if (_toRelease != null) _toRelease._semaphore.Release();
            }
        }

        /// <summary>
        /// Acquires the lock. The returned task will execute when the lock
        /// is finally acquired.
        /// </summary>
        public async Task<Releaser> Lock()
        {
            await _semaphore.Wait();
            return new Releaser(this);
        }
    }
}
