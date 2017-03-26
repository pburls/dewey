using System;
using Dewey.Deploy.Events;
using Dewey.Messaging;
using Dewey.Manifest.Dependency;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Dewey.Manifest.Messages;
using Dewey.Manifest.Models;
using Dewey.Deploy.Models;

namespace Dewey.Deploy
{
    public class DeployCommandHandler :
        ICommandHandler<DeployCommand>,
        IEventHandler<GetComponentResult>
    {
        readonly ICommandProcessor _commandProcessor;
        readonly IEventAggregator _eventAggregator;
        readonly IDeploymentActionFactory _deploymentActionFactory;

        readonly List<DependencyElementResult> _dependencies;

        DeployCommand _command;
        Component _component;

        public DeployCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator, IDeploymentActionFactory deploymentActionFactory)
        {
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;
            _deploymentActionFactory = deploymentActionFactory;

            _dependencies = new List<DependencyElementResult>();

            eventAggregator.Subscribe(this);
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

            if (!_component.IsDeployable())
            {
                _eventAggregator.PublishEvent(new NoJsonDeployManifestFound(_component));
                return false;
            }

            var deployableComponent = new DeployableComponent(_component);
            var deploy = deployableComponent.deploy;
            var deployAction = _deploymentActionFactory.CreateDeploymentAction(deploy.type);
            if (deployAction == null)
            {
                _eventAggregator.PublishEvent(new JsonDeployManifestInvalidType(_component, deploy));
                return false;
            }

            if (_command.DeployDependencies)
            {
                var componentDependencies = _component.dependencies.Where(d => d.type == ComponentDependency.COMPONENT_DEPENDENCY_TYPE && !string.IsNullOrWhiteSpace(d.name));
                foreach (var componentDependency in componentDependencies)
                {
                    _commandProcessor.Execute(DeployCommand.Create(componentDependency.name, _command.DeployDependencies));
                }
            }

            var result = false;
            try
            {
                result = deployAction.Deploy(_component, deploy);
            }
            catch (Exception ex)
            {
                _eventAggregator.PublishEvent(new JsonDeploymentActionErrorResult(_component, deploy, ex));
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
