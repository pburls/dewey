using Dewey.Manifest.Models;
using Newtonsoft.Json.Linq;

namespace Dewey.Graph.Models
{
    public class ComponentDependency : Dependency
    {
        public const string COMPONENT_DEPENDENCY_TYPE = "component";

        public string protocol { get { return (string)BackingData["protocol"]; } set { BackingData["protocol"] = value; } }

        public ComponentDependency() { }

        public ComponentDependency(Dependency dependancy) : base(dependancy.BackingData) { }

        public ComponentDependency(JObject data) : base(data) { }
    }
}
