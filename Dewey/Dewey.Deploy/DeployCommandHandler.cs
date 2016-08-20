using System;
using Dewey.Deploy.Events;
using Dewey.Manifest;
using Dewey.Manifest.Component;
using Dewey.Messaging;
using System.Linq;

namespace Dewey.Deploy
{
    public class DeployCommandHandler :
        ICommandHandler<DeployCommand>,
        IEventHandler<ComponentManifestLoadResult>,
        IEventHandler<DeploymentElementResult>
    {
        readonly ICommandProcessor _commandProcessor;
        readonly IEventAggregator _eventAggregator;

        DeployCommand _command;
        ComponentManifestLoadResult _componentMandifestLoadResult;

        public DeployCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator)
        {
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;

            eventAggregator.Subscribe<ComponentManifestLoadResult>(this);
            eventAggregator.Subscribe<DeploymentElementResult>(this);
        }

        public void Execute(DeployCommand command)
        {
            _command = command;

            _commandProcessor.Execute(new LoadManifestFiles());

            _eventAggregator.PublishEvent(new DeployCommandStarted(command));

            if (_componentMandifestLoadResult == null)
            {
                _eventAggregator.PublishEvent(new ComponentNotFoundResult(command));
                return;
            }

            LoadDeployActionsFromComponentMandifest();
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

        public void Handle(DeploymentElementResult deploymentElementResult)
        {
            try
            {
                var deploymentAction = DeploymentActionFactory.CreateDeploymentAction(deploymentElementResult.DeploymentType, _eventAggregator);
                deploymentAction.Deploy(_componentMandifestLoadResult.ComponentManifest, deploymentElementResult.DeploymentElement);
            }
            catch (Exception ex)
            {
                _eventAggregator.PublishEvent(new DeploymentActionErrorResult(_componentMandifestLoadResult.ComponentManifest, deploymentElementResult.DeploymentType, ex));
            }
        }

        private void LoadDeployActionsFromComponentMandifest()
        {
            var componentElement = _componentMandifestLoadResult.ComponentElement;

            var deploymentsElements = componentElement.Elements().FirstOrDefault(x => x.Name.LocalName == "deployments");
            if (deploymentsElements == null)
            {
                _eventAggregator.PublishEvent(new NoDeploymentElementsFoundResult(_command, componentElement));
                return;
            }

            var deploymentElements = deploymentsElements.Elements().Where(x => x.Name.LocalName == "deployment").ToList();
            if (deploymentElements.Count == 0)
            {
                _eventAggregator.PublishEvent(new NoDeploymentElementsFoundResult(_command, componentElement));
                return;
            }

            foreach (var deploymentElement in deploymentElements)
            {
                var deploymentTypeAtt = deploymentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "type");
                if (deploymentTypeAtt == null || string.IsNullOrWhiteSpace(deploymentTypeAtt.Value))
                {
                    _eventAggregator.PublishEvent(new DeploymentElementMissingTypeAttributeResult(_command, deploymentElement));
                    continue;
                }

                _eventAggregator.PublishEvent(new DeploymentElementResult(_command, deploymentElement, deploymentTypeAtt.Value));
            }
        }
    }
}
