using Dewey.Messaging;
using System;
using System.Linq;
using System.Text;

namespace Dewey.Deploy
{
    public class DeployCommand : ICommand, IEquatable<DeployCommand>
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

        public override string ToString()
        {
            var switchesBuilder = new StringBuilder();
            if (DeployDependencies) switchesBuilder.Append(" -d");

            return string.Format("{0} {1}{2}", COMMAND_TEXT, ComponentName, switchesBuilder.ToString());
        }

        public bool Equals(DeployCommand other)
        {
            if(other == null) return false;

            return base.Equals(other)
                && ComponentName == other.ComponentName
                && DeployDependencies == other.DeployDependencies;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            DeployCommand other = obj as DeployCommand;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ ComponentName.GetHashCode() ^ DeployDependencies.GetHashCode();
        }

        public static bool operator ==(DeployCommand a, DeployCommand b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.ComponentName == b.ComponentName
                && a.DeployDependencies == b.DeployDependencies;
        }

        public static bool operator !=(DeployCommand a, DeployCommand b)
        {
            return !(a == b);
        }
    }
}
