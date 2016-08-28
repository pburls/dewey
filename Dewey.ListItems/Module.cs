using Dewey.Messaging;

namespace Dewey.ListItems
{
    public class Module : IModule
    {
        public Module(ICommandProcessor commandProcessor)
        {
            commandProcessor.RegisterHandler<ListItemsCommand, ListItemsCommandHandler>();
        }
    }
}
