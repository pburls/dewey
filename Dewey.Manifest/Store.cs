﻿using Dewey.Manifest.Events;
using Dewey.Messaging;
using System.Collections.Generic;
using Dewey.Manifest.Messages;

namespace Dewey.Manifest
{
    public class Store : 
        IEventHandler<JsonManifestLoadResult>,
        ICommandHandler<GetComponent>,
        ICommandHandler<GetRuntimeResources>,
        ICommandHandler<GetComponents>
    {
        private readonly IEventAggregator _eventAggregator;

        private readonly Dictionary<string, Models.Component> _componentsDictionary;

        private readonly Dictionary<string, Models.RuntimeResource> _runtimeResourcesDictionary;

        public Store(IEventAggregator eventAggregator, ICommandProcessor commandProcessor)
        {
            _eventAggregator = eventAggregator;

            _componentsDictionary = new Dictionary<string, Models.Component>();
            _runtimeResourcesDictionary = new Dictionary<string, Models.RuntimeResource>();

            //todo: should be able to make a register all.
            commandProcessor.RegisterHandler<GetComponent, Store>();
            commandProcessor.RegisterHandler<GetRuntimeResources, Store>();
            commandProcessor.RegisterHandler<GetComponents, Store>();

            _eventAggregator.SubscribeAll(this);
        }

        public void Handle(JsonManifestLoadResult loadResult)
        {
            if (loadResult.Manifest.components != null)
            {
                foreach (var component in loadResult.Manifest.components)
                {
                    component.File = loadResult.ManifestFile; //todo: could this not just be a location string set by the loader rather?
                    _componentsDictionary.Add(component.name, component);
                }
            }

            if (loadResult.Manifest.runtimeResources != null)
            {
                foreach (var runtimeResource in loadResult.Manifest.runtimeResources)
                {
                    runtimeResource.File = loadResult.ManifestFile; //todo: could this not just be a location string set by the loader rather?
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

        public void Execute(GetRuntimeResources command)
        {
            _eventAggregator.PublishEvent(new GetRuntimeResourcesResult(command, _runtimeResourcesDictionary));
        }

        public void Execute(GetComponents command)
        {
            _eventAggregator.PublishEvent(new GetComponentsResult(command, _componentsDictionary.Values));
        }
    }
}
