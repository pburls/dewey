using Dewey.Build.Events;
using Dewey.Manifest;
using Dewey.Manifest.Component;
using Dewey.Messaging;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dewey.Build
{
    public class BuildCommandHandler : 
        ICommandHandler<BuildCommand>,
        IEventHandler<ComponentManifestLoadResult>,
        IEventHandler<BuildElementResult>,
        IEventHandler<DependencyElementResult>
    {
        readonly Container _container;
        readonly ICommandProcessor _commandProcessor;
        readonly IEventAggregator _eventAggregator;
        readonly List<DependencyElementResult> _dependencies;

        BuildCommand _command;
        ComponentManifestLoadResult _componentMandifestLoadResult;
        BuildElementResult _buildElementResult;

        public BuildCommandHandler(Container container, ICommandProcessor commandProcessor, IEventAggregator eventAggregator)
        {
            _container = container;
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;

            _dependencies = new List<DependencyElementResult>();

            eventAggregator.Subscribe<ComponentManifestLoadResult>(this);
            eventAggregator.Subscribe<BuildElementResult>(this);
            eventAggregator.Subscribe<DependencyElementResult>(this);
        }

        public void Execute(BuildCommand command)
        {
            _command = command;

            _commandProcessor.Execute(new LoadManifestFiles());

            _eventAggregator.PublishEvent(new BuildCommandStarted(command));

            if (_componentMandifestLoadResult == null)
            {
                _eventAggregator.PublishEvent(new ComponentNotFoundResult(command));
                return;
            }

            BuildElementResult.LoadBuildElementsFromComponentManifest(command, _componentMandifestLoadResult.ComponentElement, _eventAggregator);

            if (_buildElementResult == null)
            {
                return;
            }

            DependencyLoader.LoadDependencies(_componentMandifestLoadResult.ComponentElement, _eventAggregator);

            if (_dependencies.Any())
            {
                //build the dependencies first?
                foreach (var dependency in _dependencies)
                {
                    if (dependency.Type == DependencyElementResult.COMPONENT_DEPENDENCY_TYPE)
                    {
                        _commandProcessor.Execute(BuildCommand.Create(dependency.Name));
                    }
                }
            }

            try
            {
                var buildAction = BuildActionFactory.CreateBuildAction(_buildElementResult.BuildType, _container);
                buildAction.Build(_componentMandifestLoadResult.ComponentManifest, _buildElementResult.BuildElement);
            }
            catch (Exception ex)
            {
                _eventAggregator.PublishEvent(new BuildActionErrorResult(_componentMandifestLoadResult.ComponentManifest, _buildElementResult.BuildType, ex));
            }
        }

        public void Handle(ComponentManifestLoadResult componentManifestLoadResult)
        {
            if (componentManifestLoadResult.IsSuccessful)
            {
                if (componentManifestLoadResult.ComponentManifest.Name == _command.ComponentName)
                {
                    _componentMandifestLoadResult = componentManifestLoadResult;
                }
            }
        }

        public void Handle(DependencyElementResult dependencyElementResult)
        {
            _dependencies.Add(dependencyElementResult);
        }

        public void Handle(BuildElementResult buildElementResult)
        {
            if (buildElementResult.ComponentName == _command.ComponentName)
            {
                _buildElementResult = buildElementResult;
            }
        }
    }
}
