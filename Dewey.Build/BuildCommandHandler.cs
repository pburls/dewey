using Dewey.Build.Events;
using Dewey.Manifest.Dependency;
using Dewey.Messaging;
using Dewey.State;
using Dewey.State.Messages;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dewey.Build
{
    public class BuildCommandHandler : 
        ICommandHandler<BuildCommand>,
        IEventHandler<GetComponentResult>,
        IEventHandler<BuildElementResult>,
        IEventHandler<DependencyElementResult>
    {
        readonly ICommandProcessor _commandProcessor;
        readonly IEventAggregator _eventAggregator;
        readonly IBuildActionFactory _buildActionFactory;
        readonly IBuildElementLoader _buildElementLoader;
        readonly IDependencyElementLoader _dependencyElementLoader;

        readonly List<DependencyElementResult> _dependencies;

        BuildCommand _command;
        BuildElementResult _buildElementResult;
        Component _component;

        public BuildCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator, IBuildActionFactory buildActionFactory, IBuildElementLoader buildElementLoader, IDependencyElementLoader dependencyElementLoader)
        {
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;
            _buildActionFactory = buildActionFactory;
            _buildElementLoader = buildElementLoader;
            _dependencyElementLoader = dependencyElementLoader;

            _dependencies = new List<DependencyElementResult>();

            eventAggregator.Subscribe<GetComponentResult>(this);
            eventAggregator.Subscribe<BuildElementResult>(this);
            eventAggregator.Subscribe<DependencyElementResult>(this);
        }

        public void Execute(BuildCommand command)
        {
            _command = command;
            _eventAggregator.PublishEvent(new BuildCommandStarted(command));

            var stopwatch = Stopwatch.StartNew();
            var result = Execute();
            stopwatch.Stop();

            _eventAggregator.PublishEvent(new BuildCommandCompleted(command, result, stopwatch.Elapsed));
        }

        private bool Execute()
        {
            _commandProcessor.Execute(new GetComponent(_command.ComponentName));

            if (_component == null)
            {
                _eventAggregator.PublishEvent(new ComponentNotFoundResult(_command));
                return false;
            }

            _buildElementLoader.LoadFromComponentManifest(_command, _component.ComponentElement);

            if (_buildElementResult == null)
            {
                return false;
            }

            if (_command.BuildDependencies)
            {
                _dependencyElementLoader.LoadFromComponentManifest(_component.ComponentElement);

                if (_dependencies.Any())
                {
                    foreach (var dependency in _dependencies)
                    {
                        if (dependency.Type == DependencyElementResult.COMPONENT_DEPENDENCY_TYPE)
                        {
                            _commandProcessor.Execute(new BuildCommand(dependency.Name, _command.BuildDependencies));
                        }
                    }
                }
            }

            var result = false;
            try
            {
                var buildAction = _buildActionFactory.CreateBuildAction(_buildElementResult.BuildType);
                result = buildAction.Build(_component.ComponentManifest, _buildElementResult.BuildElement);
            }
            catch (Exception ex)
            {
                _eventAggregator.PublishEvent(new BuildActionErrorResult(_component.ComponentManifest, _buildElementResult.BuildType, ex));
            }

            return result;
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
