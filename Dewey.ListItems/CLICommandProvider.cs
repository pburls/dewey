using Dewey.Messaging;
using System;
using System.Collections.Generic;

namespace Dewey.ListItems
{
    public class CLICommandProvider : ICLICommandProvider
    {
        public IEnumerable<string> CommandWords
        {
            get
            {
                yield return ListItemsCommand.COMMAND_TEXT;
            }
        }

        public ICommand CreateCommand(string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("The args array must contain at least one value.", "args");

            string commandWord = args[0];
            if (commandWord == ListItemsCommand.COMMAND_TEXT)
            {
                return ListItemsCommand.Create(args);
            }

            return null;
        }
    }
}
