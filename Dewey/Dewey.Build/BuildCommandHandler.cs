using Dewey.Build.Events;
using Dewey.Manifest;
using Dewey.Manifest.Component;
using Dewey.Messaging;
using System.Linq;
using System;

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

            _eventAggregator.PublishEvent(new BuildCommandStarted(command.ComponentName));

            if (_componentMandifestLoadResult == null)
            {
                _eventAggregator.PublishEvent(new ComponentNotFoundResult(command.ComponentName));
                return;
            }

            var componentElement = _componentMandifestLoadResult.ComponentElement;

            var buildsElement = componentElement.Elements().FirstOrDefault(x => x.Name.LocalName == "builds");
            if (buildsElement == null)
            {
                _eventAggregator.PublishEvent(new ComponentNotFoundResult(command.ComponentName));
                return;
            }

            var buildElements = buildsElement.Elements().Where(x => x.Name.LocalName == "build").ToList();
            if (buildElements.Count == 0)
            {
                _eventAggregator.PublishEvent(new ComponentNotFoundResult(command.ComponentName));
                return;
            }

            foreach (var buildElement in buildElements)
            {
                var buildTypeAtt = buildElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "type");
                if (buildTypeAtt == null || string.IsNullOrWhiteSpace(buildTypeAtt.Value))
                {
                    _eventAggregator.PublishEvent(new BuildElementMissingTypeAttributeResult(command.ComponentName, buildElement));
                    continue;
                }

                _eventAggregator.PublishEvent(new BuildElementResult(command.ComponentName, buildElement, buildTypeAtt.Value));
            }
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
                var buildAction = BuildActionFactory.CreateBuildAction(buildElementResult.BuildType);
                buildAction.Build(_componentMandifestLoadResult.ComponentManifestFile.DirectoryName, buildElementResult.BuildElement);
            }
            catch (Exception ex)
            {
                _eventAggregator.PublishEvent(new BuildCommandErrorResult(_command.ComponentName, ex));
            }
        }
    }
}
