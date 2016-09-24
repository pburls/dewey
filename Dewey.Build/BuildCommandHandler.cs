using Dewey.Build.Events;
using Dewey.Manifest.Dependency;
using Dewey.Messaging;
using Dewey.State;
using Dewey.State.Messages;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dewey.Build
{
    public class BuildCommandHandler : 
        ICommandHandler<BuildCommand>,
        IEventHandler<GetComponentResult>,
        IEventHandler<BuildElementResult>,
        IEventHandler<DependencyElementResult>
    {
        readonly Container _container;
        readonly ICommandProcessor _commandProcessor;
        readonly IEventAggregator _eventAggregator;
        readonly List<DependencyElementResult> _dependencies;

        BuildCommand _command;
        BuildElementResult _buildElementResult;
        Component _component;

        public BuildCommandHandler(Container container, ICommandProcessor commandProcessor, IEventAggregator eventAggregator)
        {
            _container = container;
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;

            _dependencies = new List<DependencyElementResult>();

            eventAggregator.Subscribe<GetComponentResult>(this);
            eventAggregator.Subscribe<BuildElementResult>(this);
            eventAggregator.Subscribe<DependencyElementResult>(this);
        }

        public void Execute(BuildCommand command)
        {
            _command = command;

            _eventAggregator.PublishEvent(new BuildCommandStarted(command));

            _commandProcessor.Execute(new GetComponent(command.ComponentName));

            if (_component == null)
            {
                _eventAggregator.PublishEvent(new ComponentNotFoundResult(command));
                return;
            }

            BuildElementResult.LoadBuildElementsFromComponentManifest(command, _component.ComponentElement, _eventAggregator);

            if (_buildElementResult == null)
            {
                return;
            }

            if (_command.BuildDependencies)
            {
                DependencyElementResult.LoadDependencies(_component.ComponentElement, _eventAggregator);

                if (_dependencies.Any())
                {
                    foreach (var dependency in _dependencies)
                    {
                        if (dependency.Type == DependencyElementResult.COMPONENT_DEPENDENCY_TYPE)
                        {
                            _commandProcessor.Execute(BuildCommand.Create(dependency.Name, _command.BuildDependencies));
                        }
                    }
                }
            }

            try
            {
                var buildAction = BuildActionFactory.CreateBuildAction(_buildElementResult.BuildType, _container);
                buildAction.Build(_component.ComponentManifest, _buildElementResult.BuildElement);
            }
            catch (Exception ex)
            {
                _eventAggregator.PublishEvent(new BuildActionErrorResult(_component.ComponentManifest, _buildElementResult.BuildType, ex));
            }
        }

        public void Handle(GetComponentResult getComponentResult)
        {
            if (getComponentResult.Component != null && getComponentResult.Component.ComponentManifest.Name == _command.ComponentName)
            {
                _component = getComponentResult.Component;
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
