using System;
using Dewey.Messaging;

namespace Dewey.Build
{
    public class BuildCommand : ICommand
    {
        public string ComponentName { get; private set; }

        BuildCommand()
        {

        }

        public static BuildCommand Create(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Not enough build action parameters.");
                return null;
            }

            return new BuildCommand() { ComponentName = args[1] };
        }
    }
}
