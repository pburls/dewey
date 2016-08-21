using Dewey.Build.Events;
using Dewey.Manifest;
using Dewey.Manifest.Component;
using Dewey.Messaging;
using System.Linq;
using System;
using System.Xml.Linq;

namespace Dewey.Build
{
    public class BuildCommandHandler : 
        ICommandHandler<BuildCommand>,
        IEventHandler<ComponentManifestLoadResult>,
        IEventHandler<BuildElementResult>
    {
        readonly ICommandProcessor _commandProcessor;
        readonly IEventAggregator _eventAggregator;

        BuildCommand _command;
        ComponentManifestLoadResult _componentMandifestLoadResult;

        public BuildCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator)
        {
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;

            eventAggregator.Subscribe<ComponentManifestLoadResult>(this);
            eventAggregator.Subscribe<BuildElementResult>(this);
        }

        public void Execute(BuildCommand command)
        {
            _command = command;

            _commandProcessor.Execute(new LoadManifestFiles());

            _eventAggregator.PublishEvent(new BuildCommandStarted(command));

            if (_componentMandifestLoadResult == null)
            {
                _eventAggregator.PublishEvent(new ComponentNotFoundResult(command));
                return;
            }

            BuildElementResult.LoadBuildElementsFromComponentManifest(command, _componentMandifestLoadResult.ComponentElement, _eventAggregator);
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

        public void Handle(BuildElementResult buildElementResult)
        {
            try
            {
                var buildAction = BuildActionFactory.CreateBuildAction(buildElementResult.BuildType, _eventAggregator);
                buildAction.Build(_componentMandifestLoadResult.ComponentManifest, buildElementResult.BuildElement);
            }
            catch (Exception ex)
            {
                _eventAggregator.PublishEvent(new BuildActionErrorResult(_componentMandifestLoadResult.ComponentManifest, buildElementResult.BuildType, ex));
            }
        }
    }
}
