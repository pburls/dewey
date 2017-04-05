using Dewey.Build.Events;
using Dewey.Build.Models;
using Dewey.Manifest.Messages;
using Dewey.Manifest.Models;
using Dewey.Messaging;
using System;
using System.Diagnostics;
using System.Linq;

namespace Dewey.Build
{
    public class BuildCommandHandler : 
        ICommandHandler<BuildCommand>,
        IEventHandler<GetComponentResult>
    {
        readonly ICommandProcessor _commandProcessor;
        readonly IEventAggregator _eventAggregator;
        readonly IBuildActionFactory _buildActionFactory;
        readonly IBuildCommandCache _buildCommandCache;

        BuildCommand _command;
        Component _component;

        public BuildCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator, IBuildActionFactory buildActionFactory, IBuildCommandCache buildCommandCache)
        {
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;
            _buildActionFactory = buildActionFactory;
            _buildCommandCache = buildCommandCache;
        }

        public void Execute(BuildCommand command)
        {
            _command = command;

            if (_buildCommandCache.IsComponentAlreadyBuilt(command.ComponentName))
            {
                _eventAggregator.PublishEvent(new BuildCommandSkipped(command));
                return;
            }
            _eventAggregator.PublishEvent(new BuildCommandStarted(command));

            _eventAggregator.Subscribe(this);
            var stopwatch = Stopwatch.StartNew();
            var result = Execute();
            stopwatch.Stop();
            _eventAggregator.Unsubscribe(this);

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

            if (_command.BuildDependencies)
            {
                var componentDependencies = _component.dependencies.Where(d => d.IsComponentDependency() && !string.IsNullOrWhiteSpace(d.name));
                foreach (var componentDependency in componentDependencies)
                {
                    _commandProcessor.Execute(new BuildCommand(componentDependency.name, _command.BuildDependencies));
                }
            }

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
    }
}
