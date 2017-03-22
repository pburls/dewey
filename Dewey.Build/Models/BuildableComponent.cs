using Dewey.Manifest.Models;
using Newtonsoft.Json.Linq;

namespace Dewey.Build.Models
{
    public class BuildableComponent : Component
    {
        public Build build { get { return new Build(BackingData["build"] as JObject); } set { BackingData["build"] = value.BackingData; } }

        public BuildableComponent() { }

        public BuildableComponent(Component component) : base(component.BackingData) { }

        public BuildableComponent(JObject data) : base(data) { }
    }
}
