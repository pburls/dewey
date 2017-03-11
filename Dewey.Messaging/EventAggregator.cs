﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dewey.Messaging
{
    public class EventAggregator : IEventAggregator
    {
        private Dictionary<Type, EventHandlerCollection> _eventHandlers = new Dictionary<Type, EventHandlerCollection>();
        private Type _eventHandlerInterfaceType = typeof(IEventHandler<>);

        public void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : IEvent
        {
            Type eventHandlerInterfaceType = typeof(IEventHandler<TEvent>);
            Subscribe(eventHandlerInterfaceType, eventHandler);
        }

        public void SubscribeAll(object eventHandler)
        {
            Type eventHandlerType = eventHandler.GetType();
            var interfaces = eventHandlerType.GetInterfaces();
            var eventHandlerInterfaceTypes = interfaces.Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == _eventHandlerInterfaceType);

            foreach (var eventHandlerInterfaceType in eventHandlerInterfaceTypes)
            {
                Subscribe(eventHandlerInterfaceType, eventHandler);
            }
        }

        protected void Subscribe(Type eventHandlerInterfaceType, object eventHandler)
        {
            var eventType = eventHandlerInterfaceType.GetGenericArguments()[0];
            EventHandlerCollection eventHandlerCollection = null;

            if (!_eventHandlers.TryGetValue(eventType, out eventHandlerCollection))
            {
                eventHandlerCollection = new EventHandlerCollection(eventHandlerInterfaceType);
                _eventHandlers.Add(eventType, eventHandlerCollection);
            }

            eventHandlerCollection.Add(eventHandler);
        }

        public void PublishEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            Type eventType = @event.GetType();

            foreach (var type in _eventHandlers.Keys)
            {
                if (type.IsAssignableFrom(eventType))
                {
                    var eventHandlerCollection = _eventHandlers[type];
                    eventHandlerCollection.ExecuteAll(@event);
                }
            }
        }

        class EventHandlerCollection
        {
            private List<object> _eventHandlers = new List<object>();
            private MethodInfo _handleMethod;

            public Type EventHandlerType { get; private set; }

            public EventHandlerCollection(Type eventHandlerType)
            {
                EventHandlerType = eventHandlerType;
                _handleMethod = eventHandlerType.GetMethod("Handle");
            }

            public void Add(object eventHandler)
            {
                _eventHandlers.Add(eventHandler);
            }

            public void ExecuteAll(object @event)
            {
                foreach (var eventHandler in _eventHandlers)
                {
                    _handleMethod.Invoke(eventHandler, new[] { @event });
                }
            }
        }
    }
}
