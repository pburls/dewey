using System;
using Dewey.Deploy.Events;
using Dewey.Messaging;
using Dewey.State.Messages;
using Dewey.State;
using Dewey.Manifest.Dependency;
using System.Collections.Generic;
using System.Linq;

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
        readonly List<DependencyElementResult> _dependencies;

        DeployCommand _command;
        Component _component;
        DeploymentElementResult _deploymentElementResult;

        public DeployCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator)
        {
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;

            _dependencies = new List<DependencyElementResult>();

            eventAggregator.Subscribe<GetComponentResult>(this);
            eventAggregator.Subscribe<DeploymentElementResult>(this);
            eventAggregator.Subscribe<DependencyElementResult>(this);
        }

        public void Execute(DeployCommand command)
        {
            _command = command;

            _eventAggregator.PublishEvent(new DeployCommandStarted(command));

            _commandProcessor.Execute(new GetComponent(command.ComponentName));

            if (_component == null)
            {
                _eventAggregator.PublishEvent(new ComponentNotFoundResult(command));
                return;
            }

            DeploymentElementResult.LoadDeployActionsFromComponentMandifest(command, _component.ComponentElement, _eventAggregator);

            if (_deploymentElementResult == null)
            {
                return;
            }

            if (_command.DeployDependencies)
            {
                DependencyElementResult.LoadDependencies(_component.ComponentElement, _eventAggregator);

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

            try
            {
                var deploymentAction = DeploymentActionFactory.CreateDeploymentAction(_deploymentElementResult.DeploymentType, _eventAggregator);
                deploymentAction.Deploy(_component.ComponentManifest, _deploymentElementResult.DeploymentElement);
            }
            catch (Exception ex)
            {
                _eventAggregator.PublishEvent(new DeploymentActionErrorResult(_component.ComponentManifest, _deploymentElementResult.DeploymentType, ex));
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

        public void Handle(DeploymentElementResult deploymentElementResult)
        {
            if (deploymentElementResult.ComponentName == _command.ComponentName)
            {
                _deploymentElementResult = deploymentElementResult;
            }
        }
    }
}
