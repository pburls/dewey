using Dewey.Messaging;
using System;

namespace Dewey.Deploy
{
    public class DeployCommand : ICommand
    {
        public const string COMMAND_TEXT = "deploy";

        public string ComponentName { get; private set; }

        DeployCommand()
        {

        }

        public static DeployCommand Create(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: dewey deploy <componentName>");
                return null;
            }

            return new DeployCommand() { ComponentName = args[1] };
        }
    }
}
