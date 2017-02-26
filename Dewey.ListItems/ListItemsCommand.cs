using Dewey.Messaging;

namespace Dewey.ListItems
{
    public class ListItemsCommand : ICommand
    {
        public const string COMMAND_TEXT = "list";

        public static ListItemsCommand Create(string[] args)
        {
            return new ListItemsCommand();
        }
    }
}
