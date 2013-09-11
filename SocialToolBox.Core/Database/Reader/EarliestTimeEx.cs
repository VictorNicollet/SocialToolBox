using System;
using SocialToolBox.Core.Database.Serialization;

namespace SocialToolBox.Core.Database.Reader
{
    /// <summary>
    /// Reads time from events, stores the earliest time.
    /// </summary>
    public abstract class EarliestTimeEx<T> : IEventReader<T> where T : class
    {
        /// <summary>
        /// The earliest time found in any of the events.
        /// </summary>
        [PersistMember(0)]
        public DateTime? Time { get; protected set; }

        protected EarliestTimeEx()
        {
            Time = null;
        }

        protected EarliestTimeEx(DateTime? time)
        {
            Time = time;
            if (Time != null) Time = ((DateTime) Time).ToUniversalTime();
        }

        /// <summary>
        /// Should return the date of an event, if any.
        /// </summary>
        public abstract DateTime? EventTime(T ev);

        public void Read(T ev)
        {
            var time = EventTime(ev);
            if (time == null) return;

            var newTime = ((DateTime) time).ToUniversalTime();
            if (Time == null || Time > newTime)
            {
                Time = newTime;
            }
        }

        public override bool Equals(object objOther)
        {
            if (objOther.GetType() != GetType()) return false;
            var other = (EarliestTimeEx<T>)objOther;
            return Time == other.Time;
        }

        public override int GetHashCode()
        {
            return Time.GetHashCode();
        }

        public override string ToString()
        {
            if (Time == null) return "<UNDEFINED>";
            var date = (DateTime)Time;
            return date.ToUniversalTime().ToString("O");
        }
    }
}
