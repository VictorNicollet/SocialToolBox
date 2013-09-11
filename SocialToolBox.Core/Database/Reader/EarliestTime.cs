using System;
using SocialToolBox.Core.Database.Event;

namespace SocialToolBox.Core.Database.Reader
{
    /// <summary>
    /// Stores the earliest time among all events that satisfy a predicate.
    /// </summary>
    public abstract class EarliestTime<T> : EarliestTimeEx<T> where T : class, IEventWithTime
    {
        protected EarliestTime() {} 

        protected EarliestTime(DateTime? time) : base(time) {}

        public abstract bool AcceptEvent(T ev);

        public override DateTime? EventTime(T ev)
        {
            return AcceptEvent(ev) ? (DateTime?)ev.Time : null;            
        }
    }
}
