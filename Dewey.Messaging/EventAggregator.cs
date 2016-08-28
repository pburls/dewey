using System;
using System.Collections.Generic;

namespace Dewey.Messaging
{
    class EventAggregator : IEventAggregator
    {
        private Dictionary<Type, object> _eventHandlers = new Dictionary<Type, object>();

        public void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : IEvent
        {
            Type eventType = typeof(TEvent);
            object item = null;
            EventHandlerCollection<TEvent> eventHandlerCollection = null;

            if(_eventHandlers.TryGetValue(eventType, out item))
            {
                eventHandlerCollection = (EventHandlerCollection<TEvent>)item;
            }
            else
            {
                eventHandlerCollection = new EventHandlerCollection<TEvent>();
                _eventHandlers.Add(eventType, eventHandlerCollection);
            }

            eventHandlerCollection.Add(eventHandler);
        }

        public void PublishEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            Type eventType = typeof(TEvent);

            if (_eventHandlers.ContainsKey(eventType))
            {
                ((EventHandlerCollection<TEvent>)_eventHandlers[eventType]).ExecuteAll(@event);
            }
        }

        class EventHandlerCollection<TEvent> where TEvent : IEvent
        {
            private List<IEventHandler<TEvent>> _eventHandlers = new List<IEventHandler<TEvent>>();

            public void Add(IEventHandler<TEvent> eventHandler)
            {
                _eventHandlers.Add(eventHandler);
            }

            public void ExecuteAll(TEvent @event)
            {
                foreach (var eventHandler in _eventHandlers)
                {
                    eventHandler.Handle(@event);
                }
            }
        }
    }
}
