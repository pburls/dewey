using System;
using Dewey.Messaging;
using System.Linq;
using System.Text;

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

        public override string ToString()
        {
            var switchesBuilder = new StringBuilder();
            if (BuildDependencies) switchesBuilder.Append(" -d");

            return string.Format("{0} {1}{2}", COMMAND_TEXT, ComponentName, switchesBuilder.ToString());
        }
    }
}
