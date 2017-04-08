using Ark3.Event;
using Dewey.Messaging.Events;
using SimpleInjector;
using System;
using System.Collections.Generic;

namespace Dewey.Messaging
{
    public class ModuleCatalogue
    {
        readonly Container _container;
        readonly IEventAggregator _eventAggregator;

        readonly Dictionary<Type, IModule> _moduleDictionary;

        public ModuleCatalogue(Container container, IEventAggregator eventAggregator)
        {
            _container = container;
            _eventAggregator = eventAggregator;

            _moduleDictionary = new Dictionary<Type, IModule>();
        }

        public void Load<TModule>() where TModule : class, IModule
        {
            var moduleType = typeof(TModule);

            var module = _container.GetInstance<TModule>();

            _moduleDictionary.Add(moduleType, module);

            _eventAggregator.PublishEvent(new ModuleLoaded(module));
        }
    }
}
