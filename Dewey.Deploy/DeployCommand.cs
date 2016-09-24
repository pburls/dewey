using Dewey.Messaging;
using System;
using System.Linq;

namespace Dewey.Deploy
{
    public class DeployCommand : ICommand
    {
        public const string COMMAND_TEXT = "deploy";

        public string ComponentName { get; private set; }

        public bool DeployDependencies { get; private set; }

        DeployCommand()
        {

        }

        public static DeployCommand Create(string[] args)
        {
            var arguments = args.Skip(1);
            var componentName = arguments.Where(arg => !arg.StartsWith("-")).FirstOrDefault();
            if (string.IsNullOrEmpty(componentName))
            {
                Console.WriteLine("Usage: dewey deploy <componentName> [switches]");
                Console.WriteLine("Switches:");
                Console.WriteLine(" -d     : First deploy all the component's dependencies and any of the dependencies' dependencies.");
                return null;
            }

            var switches = arguments.Where(arg => arg.StartsWith("-"));
            var buildDependencies = switches.Any(s => s.Contains("d"));

            return Create(componentName, buildDependencies);
        }

        public static DeployCommand Create(string componentName, bool deployDependencies)
        {
            return new DeployCommand() { ComponentName = componentName, DeployDependencies = deployDependencies };
        }
    }
}
