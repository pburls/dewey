using Dewey.Manifest.Models;
using Newtonsoft.Json.Linq;

namespace Dewey.Deploy.Models
{
    public class DeployableComponent : Component
    {
        public Deploy deploy { get { return new Deploy(BackingData["deploy"] as JObject); } set { BackingData["deploy"] = value.BackingData; } }

        public DeployableComponent() { }

        public DeployableComponent(Component component) : base(component.BackingData) { }

        public DeployableComponent(JObject data) : base(data) { }
    }
}
