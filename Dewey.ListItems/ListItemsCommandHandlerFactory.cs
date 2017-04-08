using Ark3.Event;
using Ark3.Command;

namespace Dewey.ListItems
{
    class ListItemsCommandHandlerFactory : ICommandHandlerFactory<ListItemsCommand>
    {
        readonly IEventAggregator _eventAggregator;
        readonly ICommandProcessor _commandProcessor;

        public ListItemsCommandHandlerFactory(IEventAggregator eventAggregator, ICommandProcessor commandProcessor)
        {
            _eventAggregator = eventAggregator;
            _commandProcessor = commandProcessor;
        }

        public ICommandHandler<ListItemsCommand> CreateHandler()
        {
            return new ListItemsCommandHandler(_commandProcessor, _eventAggregator);
        }
    }
}
