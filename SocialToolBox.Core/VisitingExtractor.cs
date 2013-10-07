using System;
using System.Collections.Generic;

namespace SocialToolBox.Core
{
    /// <summary>
    /// Acts as a visitor, but does not expect any input.
    /// </summary>
    public class VisitingExtractor<TIPoly, TOutput> where TIPoly : class
    {
        /// <summary>
        /// A visitor action. These are stored for each type.
        /// </summary>
        private delegate TOutput VisitorAction(object e);

        /// <summary>
        /// The visitor action registered for each type.
        /// </summary>
        private readonly Dictionary<Type,VisitorAction> _funcs = 
            new Dictionary<Type,VisitorAction>();

        /// <summary>
        /// The action performed if no other action was found. 
        /// </summary>
        private VisitorAction _defaultAction;

        /// <summary>
        /// Registers a new event handler for the specified event type.
        /// </summary>
        /// <remarks>
        /// For class types, only instances of that class (not derived 
        /// classes) are affected. For interfaces, all classes that
        /// implement that interface are affected, unless they have
        /// their own binding.
        /// </remarks>
        public void On<TEvent>(Func<TEvent, TOutput> f) where TEvent : TIPoly
        {
            try
            {
                _funcs.Add(typeof (TEvent), e => f((TEvent) e));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Could not register visitor.", e);
            }
        }

        /// <summary>
        /// Specifies a default action for the visitor.
        /// </summary>
        public void SetDefaultAction(Func<object, TOutput> f)
        {
            _defaultAction = e => f(e);
        }

        /// <summary>
        /// Finds the visitor action for a specified type.
        /// </summary>
        private void FindForType(Type t, out VisitorAction action)
        {
            if (_funcs.TryGetValue(t, out action)) return;

            foreach (var i in t.GetInterfaces())
                if (_funcs.TryGetValue(i, out action)) return;

            action = _defaultAction;
        }

        /// <summary>
        /// Visits the object, determining the required action based on its type.
        /// Action receives input data and object, its return value is returned. 
        /// </summary>
        public TOutput Visit(TIPoly e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            VisitorAction action;
            FindForType(e.GetType(), out action);
            
            if (action == null)
                throw new ArgumentException(
                    string.Format("No visitor action for type {0}", e.GetType()),
                    "e");
                    
            return action(e);    
        }
    }
}
