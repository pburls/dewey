using System;
using Dewey.Manifest.Events;
using Dewey.Messaging;
using System.Collections.Generic;
using Dewey.Manifest.Messages;

namespace Dewey.Manifest
{
    public class Store : 
        IEventHandler<JsonManifestLoadResult>,
        ICommandHandler<GetComponent>
    {
        private readonly IEventAggregator _eventAggregator;

        private readonly Dictionary<string, Models.Component> _componentsDictionary;

        private readonly Dictionary<string, Models.RuntimeResource> _runtimeResourcesDictionary;

        public Store(IEventAggregator eventAggregator, ICommandProcessor commandProcessor)
        {
            _eventAggregator = eventAggregator;

            _componentsDictionary = new Dictionary<string, Models.Component>();
            _runtimeResourcesDictionary = new Dictionary<string, Models.RuntimeResource>();

            commandProcessor.RegisterHandler<GetComponent, Store>();

            _eventAggregator.SubscribeAll(this);
        }

        public void Handle(JsonManifestLoadResult loadResult)
        {
            if (loadResult.Manifest.components != null)
            {
                foreach (var component in loadResult.Manifest.components)
                {
                    _componentsDictionary.Add(component.name, component);
                }
            }

            if (loadResult.Manifest.components != null)
            {
                foreach (var runtimeResource in loadResult.Manifest.runtimeResources)
                {
                    _runtimeResourcesDictionary.Add(runtimeResource.name, runtimeResource);
                }
            }
        }

        public void Execute(GetComponent command)
        {
            Models.Component component = null;
            _componentsDictionary.TryGetValue(command.ComponentName, out component);

            _eventAggregator.PublishEvent(new GetComponentResult(command, component));
        }
    }
}
