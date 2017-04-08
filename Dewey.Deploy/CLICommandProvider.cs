using Ark3.Command;
using Dewey.Messaging;
using System;
using System.Collections.Generic;

namespace Dewey.Deploy
{
    public class CLICommandProvider : ICLICommandProvider
    {
        public IEnumerable<string> CommandWords
        {
            get
            {
                yield return DeployCommand.COMMAND_TEXT;
            }
        }

        public ICommand CreateCommand(string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("The args array must contain at least one value.", "args");

            string commandWord = args[0];
            if (commandWord == DeployCommand.COMMAND_TEXT)
            {
                return DeployCommand.Create(args);
            }

            return null;
        }
    }
}
