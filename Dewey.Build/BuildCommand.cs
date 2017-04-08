using System;
using Ark3.Command;
using System.Linq;
using System.Text;

namespace Dewey.Build
{
    public class BuildCommand : ICommand, IEquatable<BuildCommand>
    {
        public const string COMMAND_TEXT = "build";

        public string ComponentName { get; private set; }

        public bool BuildDependencies { get; private set; }

        public BuildCommand(string componentName, bool buildDependencies)
        {
            ComponentName = componentName;
            BuildDependencies = buildDependencies;
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

            return new BuildCommand(componentName, buildDependencies);
        }

        public override string ToString()
        {
            var switchesBuilder = new StringBuilder();
            if (BuildDependencies) switchesBuilder.Append(" -d");

            return string.Format("{0} {1}{2}", COMMAND_TEXT, ComponentName, switchesBuilder.ToString());
        }

        public bool Equals(BuildCommand other)
        {
            if (other == null) return false;

            return ComponentName == other.ComponentName && BuildDependencies == other.BuildDependencies;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            BuildCommand other = obj as BuildCommand;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return ComponentName.GetHashCode() ^ BuildDependencies.GetHashCode();
        }

        public static bool operator ==(BuildCommand a, BuildCommand b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.ComponentName == b.ComponentName && a.BuildDependencies == b.BuildDependencies;
        }

        public static bool operator !=(BuildCommand a, BuildCommand b)
        {
            return !(a == b);
        }
    }
}
