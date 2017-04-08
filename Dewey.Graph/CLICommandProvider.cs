using Ark3.Command;
using Dewey.Messaging;
using System;
using System.Collections.Generic;

namespace Dewey.Graph
{
    public class CLICommandProvider : ICLICommandProvider
    {
        public IEnumerable<string> CommandWords
        {
            get
            {
                yield return GraphCommand.COMMAND_TEXT;
            }
        }

        public ICommand CreateCommand(string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("The args array must contain at least one value.", "args");

            string commandWord = args[0];
            if (commandWord == GraphCommand.COMMAND_TEXT)
            {
                return GraphCommand.Create(args);
            }

            return null;
        }
    }
}
