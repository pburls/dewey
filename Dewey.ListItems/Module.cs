using Dewey.Messaging;

namespace Dewey.ListItems
{
    public class Module : IModule
    {
        public Module(IEventAggregator eventAggregator, ICommandProcessor commandProcessor)
        {
            var listItemsCommandHandlerFactory = new ListItemsCommandHandlerFactory(eventAggregator, commandProcessor);
            commandProcessor.RegisterHandlerFactory<ListItemsCommand, ListItemsCommandHandlerFactory>(listItemsCommandHandlerFactory);
        }
    }
}
