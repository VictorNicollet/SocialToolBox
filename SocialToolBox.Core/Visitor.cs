using System;
using System.Collections.Generic;

namespace SocialToolBox.Core
{
    /// <summary>
    /// A reflection-based implementation of a visitor pattern.
    /// </summary>
    public class Visitor<TInput,TOutput>
    {
        /// <summary>
        /// A visitor action. These are stored for each type.
        /// </summary>
        private delegate TOutput VisitorAction(object e, TInput input);

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
        public void On<TEvent>(Func<TEvent, TInput, TOutput> f)
        {
            try
            {
                _funcs.Add(typeof (TEvent), (e, i) => f((TEvent) e, i));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Could not register visitor.", e);
            }
        }

        /// <summary>
        /// Specifies a default action for the visitor.
        /// </summary>
        public void SetDefaultAction(Func<object, TInput, TOutput> f)
        {
            _defaultAction = (e,i) => f(e,i);
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
        public TOutput Visit(object e, TInput input)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            VisitorAction action;
            FindForType(e.GetType(), out action);
            
            if (action == null)
                throw new ArgumentException(
                    string.Format("No visitor action for type {0}", e.GetType()),
                    "e");
                    
            return action(e, input);    
        }
    }
}
