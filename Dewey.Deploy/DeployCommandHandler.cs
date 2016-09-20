using System;
using Dewey.Deploy.Events;
using Dewey.Messaging;
using Dewey.State.Messages;
using Dewey.State;

namespace Dewey.Deploy
{
    public class DeployCommandHandler :
        ICommandHandler<DeployCommand>,
        IEventHandler<GetComponentResult>,
        IEventHandler<DeploymentElementResult>
    {
        readonly ICommandProcessor _commandProcessor;
        readonly IEventAggregator _eventAggregator;

        DeployCommand _command;
        Component _component;
        DeploymentElementResult _deploymentElementResult;

        public DeployCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator)
        {
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;

            eventAggregator.Subscribe<GetComponentResult>(this);
            eventAggregator.Subscribe<DeploymentElementResult>(this);
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

        public void Handle(DeploymentElementResult deploymentElementResult)
        {
            if (deploymentElementResult.ComponentName == _command.ComponentName)
            {
                _deploymentElementResult = deploymentElementResult;
            }
        }
    }
}
