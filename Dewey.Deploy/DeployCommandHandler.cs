using System;
using Dewey.Deploy.Events;
using Dewey.Messaging;
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
        readonly IDeployCommandCache _deployCommandCache;

        DeployCommand _command;
        Component _component;

        public DeployCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator, IDeploymentActionFactory deploymentActionFactory, IDeployCommandCache deployCommandCache)
        {
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;
            _deploymentActionFactory = deploymentActionFactory;
            _deployCommandCache = deployCommandCache;
        }

        public void Execute(DeployCommand command)
        {
            _command = command;

            if (_deployCommandCache.IsComponentAlreadyDeployed(command.ComponentName))
            {
                _eventAggregator.PublishEvent(new DeployCommandSkipped(command));
                return;
            }
            _eventAggregator.PublishEvent(new DeployCommandStarted(command));

            _eventAggregator.Subscribe(this);
            var stopwatch = Stopwatch.StartNew();
            var result = Execute();
            stopwatch.Stop();
            _eventAggregator.Unsubscribe(this);

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
                var componentDependencies = _component.dependencies.Where(d => d.IsComponentDependency() && !string.IsNullOrWhiteSpace(d.name));
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
