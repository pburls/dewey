using Ark3.Command;
using Dewey.Messaging;
using System;
using System.Collections.Generic;

namespace Dewey.Build
{
    public class CLICommandProvider : ICLICommandProvider
    {
        public IEnumerable<string> CommandWords
        {
            get
            {
                yield return BuildCommand.COMMAND_TEXT;
            }
        }

        public ICommand CreateCommand(string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("The args array must contain at least one value.", "args");

            string commandWord = args[0];
            if (commandWord == BuildCommand.COMMAND_TEXT)
            {
                return BuildCommand.Create(args);
            }

            return null;
        }
    }
}
