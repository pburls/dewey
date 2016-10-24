using System;
using Dewey.Deploy.Events;
using Dewey.Messaging;
using Dewey.State.Messages;
using Dewey.State;
using Dewey.Manifest.Dependency;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Dewey.Deploy
{
    public class DeployCommandHandler :
        ICommandHandler<DeployCommand>,
        IEventHandler<GetComponentResult>,
        IEventHandler<DeploymentElementResult>,
        IEventHandler<DependencyElementResult>
    {
        readonly ICommandProcessor _commandProcessor;
        readonly IEventAggregator _eventAggregator;
        readonly IDependencyElementLoader _dependencyElementLoader;
        readonly IDeploymentActionFactory _deploymentActionFactory;

        readonly List<DependencyElementResult> _dependencies;

        DeployCommand _command;
        Component _component;
        DeploymentElementResult _deploymentElementResult;

        public DeployCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator, IDependencyElementLoader dependencyElementLoader, IDeploymentActionFactory deploymentActionFactory)
        {
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;
            _dependencyElementLoader = dependencyElementLoader;
            _deploymentActionFactory = deploymentActionFactory;

            _dependencies = new List<DependencyElementResult>();

            eventAggregator.Subscribe<GetComponentResult>(this);
            eventAggregator.Subscribe<DeploymentElementResult>(this);
            eventAggregator.Subscribe<DependencyElementResult>(this);
        }

        public void Execute(DeployCommand command)
        {
            _command = command;
            _eventAggregator.PublishEvent(new DeployCommandStarted(command));

            var stopwatch = Stopwatch.StartNew();
            var result = Execute();
            stopwatch.Stop();

            _eventAggregator.PublishEvent(new DeployCommandCompleted(command, result, stopwatch.Elapsed));
        }

        private bool Execute()
        {
            _commandProcessor.Execute(new GetComponent(_command.ComponentName));

            if (_component == null)
            {
                _eventAggregator.PublishEvent(new ComponentNotFoundResult(_command));
                return false;
            }

            DeploymentElementResult.LoadDeployActionsFromComponentMandifest(_command, _component.ComponentElement, _eventAggregator);

            if (_deploymentElementResult == null)
            {
                return false;
            }

            if (_command.DeployDependencies)
            {
                _dependencyElementLoader.LoadFromComponentManifest(_component.ComponentManifest, _component.ComponentElement);

                if (_dependencies.Any())
                {
                    foreach (var dependency in _dependencies)
                    {
                        if (dependency.Type == DependencyElementResult.COMPONENT_DEPENDENCY_TYPE)
                        {
                            _commandProcessor.Execute(DeployCommand.Create(dependency.Name, _command.DeployDependencies));
                        }
                    }
                }
            }

            var result = false;
            try
            {
                var deploymentAction = _deploymentActionFactory.CreateDeploymentAction(_deploymentElementResult.DeploymentType);
                result = deploymentAction.Deploy(_component.ComponentManifest, _deploymentElementResult.DeploymentElement);
            }
            catch (Exception ex)
            {
                _eventAggregator.PublishEvent(new DeploymentActionErrorResult(_component.ComponentManifest, _deploymentElementResult.DeploymentType, ex));
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
            if (_component.ComponentManifest.Name == dependencyElementResult.ComponentManifest.Name)
            {
                _dependencies.Add(dependencyElementResult);
            }
        }

        public void Handle(DeploymentElementResult deploymentElementResult)
        {
            if (deploymentElementResult.ComponentName == _command.ComponentName)
            {
                _deploymentElementResult = deploymentElementResult;
            }
        }
    }
}
