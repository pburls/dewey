using Dewey.Build.Events;
using Dewey.Build.Models;
using Dewey.Manifest.Dependency;
using Dewey.Manifest.Messages;
using Dewey.Manifest.Models;
using Dewey.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dewey.Build
{
    public class BuildCommandHandler : 
        ICommandHandler<BuildCommand>,
        IEventHandler<GetComponentResult>,
        IEventHandler<DependencyElementResult>
    {
        readonly ICommandProcessor _commandProcessor;
        readonly IEventAggregator _eventAggregator;
        readonly IBuildActionFactory _buildActionFactory;
        readonly IBuildElementLoader _buildElementLoader;
        readonly IDependencyElementLoader _dependencyElementLoader;

        readonly List<DependencyElementResult> _dependencies;

        BuildCommand _command;
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

            if (!_component.IsBuildable())
            {
                _eventAggregator.PublishEvent(new NoJsonBuildManifestFound(_component));
                return false;
            }

            var buildableComponent = new BuildableComponent(_component);
            var build = buildableComponent.build;
            var buildAction = _buildActionFactory.CreateBuildAction(build.type);
            if (buildAction == null)
            {
                _eventAggregator.PublishEvent(new JsonBuildManifestInvalidType(_component, build));
                return false;
            }

            //if (_command.BuildDependencies)
            //{
            //    _dependencyElementLoader.LoadFromComponentManifest(_component.ComponentManifest, _component.ComponentElement);

            //    if (_dependencies.Any())
            //    {
            //        foreach (var dependency in _dependencies)
            //        {
            //            if (dependency.Type == ComponentDependency.COMPONENT_DEPENDENCY_TYPE)
            //            {
            //                _commandProcessor.Execute(new BuildCommand(dependency.Name, _command.BuildDependencies));
            //            }
            //        }
            //    }
            //}

            var result = false;
            try
            {
                result = buildAction.Build(_component, build);
            }
            catch (Exception ex)
            {
                _eventAggregator.PublishEvent(new JsonBuildActionErrorResult(_component, build, ex));
            }

            return result;
        }

        public void Handle(GetComponentResult getComponentResult)
        {
            if (getComponentResult.Component != null && getComponentResult.Component.name == _command.ComponentName)
            {
                _component = getComponentResult.Component;
            }
        }

        public void Handle(DependencyElementResult dependencyElementResult)
        {
            if (_component != null && _component.name == dependencyElementResult.ComponentManifest.Name)
            {
                _dependencies.Add(dependencyElementResult);
            }
        }
    }
}
