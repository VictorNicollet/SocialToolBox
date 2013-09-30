using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialToolBox.Core.Async
{
    /// <summary>
    /// An async-compatible semaphore.
    /// </summary>
    /// <remarks>
    /// Inspired by http://blogs.msdn.com/b/pfxteam/archive/2012/02/12/10266983.aspx
    /// </remarks>
    public class AsyncSemaphore
    {
        /// <summary>
        /// A "completed" task kept around to avoid repeated instantiation.
        /// </summary>
        private readonly static Task Completed = Task.FromResult(true);
        
        /// <summary>
        /// ALl the tasks currently waiting on this semaphore.
        /// </summary>
        private readonly Queue<TaskCompletionSource<bool>> _waiters = 
            new Queue<TaskCompletionSource<bool>>();
        
        /// <summary>
        /// The current semaphore count.
        /// </summary>
        private int _currentCount;

        public AsyncSemaphore(int initialCount = 1)
        {
            if (initialCount < 0) throw new ArgumentOutOfRangeException("initialCount"); 
            _currentCount = initialCount;
        }

        /// <summary>
        /// Waits until the count becomes greater than zero, then decrements
        /// count and returns.
        /// </summary>
        public Task Wait()
        {
            lock (_waiters)
            {
                if (_currentCount > 0)
                {
                    --_currentCount;
                    return Completed;
                }
                
                var waiter = new TaskCompletionSource<bool>();
                _waiters.Enqueue(waiter);
                return waiter.Task;           
            }
        }

        /// <summary>
        /// Releases one unit of the semaphore.
        /// </summary>
        public void Release()
        {
            TaskCompletionSource<bool> toRelease = null;
            lock (_waiters)
            {
                if (_waiters.Count > 0)
                    toRelease = _waiters.Dequeue();
                else
                    ++_currentCount;
            }

            if (toRelease != null)
                toRelease.SetResult(true);            
        }
    }
}
