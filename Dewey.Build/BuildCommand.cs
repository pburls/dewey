using System;
using Dewey.Messaging;
using System.Linq;

namespace Dewey.Build
{
    public class BuildCommand : ICommand
    {
        public const string COMMAND_TEXT = "build";

        public string ComponentName { get; private set; }

        public bool BuildDependencies { get; private set; }

        BuildCommand()
        {

        }

        public static BuildCommand Create(string[] args)
        {
            var arguments = args.Skip(1);
            var componentName = arguments.Where(arg => !arg.StartsWith("-")).FirstOrDefault();
            if (string.IsNullOrEmpty(componentName))
            {
                Console.WriteLine("Usage: dewey build <componentName> [switches]");
                Console.WriteLine("Switches:");
                Console.WriteLine(" -d     : First build all the component's dependencies and any of the dependencies' dependencies.");
                return null;
            }

            var switches = arguments.Where(arg => arg.StartsWith("-"));
            var buildDependencies = switches.Any(s => s.Contains("d"));

            return Create(componentName, buildDependencies);
        }

        public static BuildCommand Create(string componentName, bool buildDependencies)
        {
            return new BuildCommand() { ComponentName = componentName, BuildDependencies = buildDependencies };
        }
    }
}
