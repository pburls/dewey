﻿using System;
using Dewey.Manifest.Events;
using Dewey.Messaging;
using System.Collections.Generic;
using Dewey.Manifest.Messages;

namespace Dewey.Manifest
{
    public class Store : 
        IEventHandler<JsonManifestLoadResult>,
        ICommandHandler<GetComponent>,
        ICommandHandler<GetRuntimeResources>
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

        public void Execute(GetRuntimeResources command)
        {
            _eventAggregator.PublishEvent(new GetRuntimeResourcesResult(command, _runtimeResourcesDictionary));
        }
    }
}
