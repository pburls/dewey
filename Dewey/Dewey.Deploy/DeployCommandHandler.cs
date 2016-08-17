using Dewey.Manifest;
using Dewey.Messaging;

namespace Dewey.Deploy
{
    public class DeployCommandHandler :
        ICommandHandler<DeployCommand>
    {
        readonly ICommandProcessor _commandProcessor;
        readonly IEventAggregator _eventAggregator;

        DeployCommand _command;

        public DeployCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator)
        {
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;
        }

        public void Execute(DeployCommand command)
        {
            _command = command;

            _commandProcessor.Execute(new LoadManifestFiles());
        }
    }
}
